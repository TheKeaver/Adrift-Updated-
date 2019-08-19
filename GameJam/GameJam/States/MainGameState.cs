using System;
using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Input;
using GameJam.Processes;
using GameJam.States;
using GameJam.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GameJam
{
    /// <summary>
    /// Game state for the main game.
    /// </summary>
    public class MainGameState : GameState, IEventListener
    {
        private ProcessManager ProcessManager {
            get;
            set;
        }

        float _acculmulator;

        BaseSystem[] _systems;

        RenderSystem _renderSystem;

        BaseDirector[] _directors;

        Camera _mainCamera;

        public Engine Engine
        {
            get;
            private set;
        }

        Player Player;

        public MainGameState(GameManager gameManager, Player player)
            : base(gameManager)
        {
            Player = player;
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            _mainCamera = new Camera(CVars.Get<int>("window_width"), CVars.Get<int>("window_height"));
            _mainCamera.RegisterEvents();

            InitSystems();
            InitDirectors();

            ProcessManager.Attach(new KamikazeSpawner(Engine, Content));
            ProcessManager.Attach(new ShooterEnemySpawner(Engine, Content, ProcessManager));

            EventManager.Instance.RegisterListener<GameOverEvent>(this);
        }

        void InitSystems()
        {
            Engine = new Engine();

            // Order matters
            _systems = new BaseSystem[]
            {
                new InputSystem(Engine), // Input system must go first so snapshots are accurate
                new CollisionSystem(Engine),
                new PlayerShieldSystem(Engine),
                new MovementSystem(Engine),
                new CollisionSystem(Engine),
                new PlayerShieldCollisionSystem(Engine),
                new EnemyRotationSystem(Engine),
                new AnimationSystem(Engine),
                new ExplosionSystem(Engine)
            };

            _renderSystem = new RenderSystem(GameManager.GraphicsDevice, Engine);
        }
        void InitDirectors()
        {
            // Order does not matter
            _directors = new BaseDirector[]
            {
                new ShipDirector(Engine, Content, ProcessManager),
                new ShieldDirector(Engine, Content, ProcessManager),
                new SoundDirector(Engine, Content, ProcessManager),
                new ExplosionDirector(Engine, Content, ProcessManager),
                new ChangeToKamikazeDirector(Engine, Content, ProcessManager),
                new KamikazeDirector(Engine, Content, ProcessManager),
                new EnemyBulletDirector(Engine, Content, ProcessManager),
                new BulletBounceDirector(Engine, Content, ProcessManager),
                new BounceDirector(Engine, Content, ProcessManager),
                new BulletBounceListenerDirector(Engine, Content, ProcessManager)
            };
            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].RegisterEvents();
            }
        }

        public override void Hide()
        {
        }

        public override void LoadContent()
        {
            Content.Load<Texture2D>(CVars.Get<string>("texture_player_ship"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_player_shield"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_explosion"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_kamikaze"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_shooter_enemy"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_enemy_bullet"));

            Content.Load<SoundEffect>(CVars.Get<string>("sound_explosion"));
            Content.Load<SoundEffect>(CVars.Get<string>("sound_projectile_fired"));
			Content.Load<SoundEffect>(CVars.Get<string>("sound_projectile_bounce"));

            Content.Load<BitmapFont>(CVars.Get<string>("font_game_over"));
        }

        public override void Show()
        {
            CreateEntities();

            // Updates the camera and post-processor with the actual screen size
            // This fixes a bug present in a build of Super Pong
            EventManager.Instance.TriggerEvent(new ResizeEvent(GameManager.GraphicsDevice.Viewport.Width,
                                                              GameManager.GraphicsDevice.Viewport.Height));
        }

        void CreateEntities()
        {
            Entity playerShipEntity = PlayerShipEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_player_ship")),
                new Vector2(0, 0));
            Entity playerShieldEntity = PlayerShieldEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_player_shield")), playerShipEntity);
            playerShipEntity.GetComponent<PlayerShipComponent>().shipShield = playerShieldEntity;
            playerShieldEntity.AddComponent(new PlayerComponent(Player));

            Entity topEdge = EdgeEntity.Create(Engine, new Vector2(0, CVars.Get<int>("window_height") / 2), new Vector2(CVars.Get<int>("window_width"), 1), new Vector2(0,-1));
            Entity leftEdge = EdgeEntity.Create(Engine, new Vector2(-CVars.Get<int>("window_width") / 2, 0), new Vector2(1, CVars.Get<int>("window_height")), new Vector2(1, 0));
            Entity bottomEdge = EdgeEntity.Create(Engine, new Vector2(0, -CVars.Get<int>("window_width") / 2), new Vector2(CVars.Get<int>("window_width"), 1), new Vector2(0, 1));
            Entity rightEdge = EdgeEntity.Create(Engine, new Vector2(CVars.Get<int>("window_width") / 2, 0), new Vector2(1, CVars.Get<int>("window_height")), new Vector2(-1, 0));
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);

            _acculmulator += dt;
            while(_acculmulator >= 1 / CVars.Get<float>("tick_frequency"))
            {
                _acculmulator -= 1 / CVars.Get<float>("tick_frequency");

                for(int i = 0; i < _systems.Length; i++)
                {
                    _systems[i].Update(dt);
                }
            }
        }

        public override void Draw(float dt)
        {
            float betweenFrameAlpha = _acculmulator / (1 / CVars.Get<float>("tick_frequency"));

            _renderSystem.DrawEntities(_mainCamera.TransformMatrix,
                Constants.Render.GROUP_MASK_ALL,
                dt,
                betweenFrameAlpha);
        }

        public override void Dispose()
        {
            // Remove listeners
            EventManager.Instance.UnregisterListener(this);
            _mainCamera.UnregisterEvents();

            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].UnregisterEvents();
            }
        }

        public bool Handle(IEvent evt)
        {
            if(evt is GameOverEvent)
            {
                HandleGameOverEvent(evt as GameOverEvent);
            }

            return false;
        }

        private void HandleGameOverEvent(GameOverEvent gameOverEvent)
        {
            // TODO: Game Over Process
            Entity gameOverText = Engine.CreateEntity();
            gameOverText.AddComponent(new TransformComponent(new Vector2(0, 1.25f * CVars.Get<int>("window_height") / 2)));
            gameOverText.AddComponent(new FontComponent(Content.Load<BitmapFont>(CVars.Get<string>("font_game_over")), "Game Over"));

            ProcessManager.Attach(new GameOverAnimationProcess(gameOverText)).SetNext(new WaitProcess(3)).SetNext(new DelegateCommand(() =>
            {
                GameManager.ChangeState(new MenuGameState(GameManager));
            }));
        }
    }
}

