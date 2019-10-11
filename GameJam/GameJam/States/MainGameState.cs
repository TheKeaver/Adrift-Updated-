using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Events.GameLogic;
using GameJam.Graphics;
using GameJam.Particles;
using GameJam.Processes;
using GameJam.Processes.Animation;
using GameJam.Processes.Enemies;
using GameJam.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.States
{
    /// <summary>
    /// Game state for the main game.
    /// </summary>
    public class MainGameState : GameState, IEventListener
    {
        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public PostProcessor AdriftPostProcessor
        {
            get;
            private set;
        }
        private PostProcessorEffect _fxaaPPE;

        public ParticleManager<VelocityParticleInfo> VelocityParticleManager
        {
            get;
            private set;
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

        Player[] PlayerArray;

        public MainGameState(GameManager gameManager, Player[] players)
            : base(gameManager)
        {
            PlayerArray = players;
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            _mainCamera = new Camera(CVars.Get<int>("window_width"), CVars.Get<int>("window_height"));
            _mainCamera.RegisterEvents();

            AdriftPostProcessor = new PostProcessor(GameManager.GraphicsDevice,
                                                    CVars.Get<int>("window_width"),
                                                    CVars.Get<int>("window_height"));
            AdriftPostProcessor.RegisterEvents();

            VelocityParticleManager = new ParticleManager<VelocityParticleInfo>(1024 * 20, VelocityParticleInfo.UpdateParticle);

            InitSystems();
            InitDirectors();

            ProcessManager.Attach(new KamikazeSpawner(Engine, Content));
            ProcessManager.Attach(new ShooterEnemySpawner(Engine, Content, ProcessManager));
            ProcessManager.Attach(new GravityEnemySpawner(Engine, Content, ProcessManager));
            ProcessManager.Attach(new KamikazeSpawner(Engine));
            ProcessManager.Attach(new ShooterEnemySpawner(Engine, ProcessManager));
            ProcessManager.Attach(new LaserEnemySpawner(Engine, ProcessManager));

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
                new EnemyRotationSystem(Engine),
                new AnimationSystem(Engine),
                new LaserEnemySystem(Engine),
                new GravitySystem(Engine)
            };

            _renderSystem = new RenderSystem(GameManager.GraphicsDevice, Engine);
        }
        void InitDirectors()
                new LaserEnemySystem(Engine),
                new AnimationSystem(Engine),
                new ShieldDirector(Engine, Content, ProcessManager),
                new SoundDirector(Engine, Content, ProcessManager),
                new ExplosionDirector(Engine, Content, ProcessManager, VelocityParticleManager),
                new ChangeToKamikazeDirector(Engine, Content, ProcessManager),
                new EnemyCollisionOnPlayerDirector(Engine, Content, ProcessManager),
                new HazardCollisionOnEnemyDirector(Engine, Content, ProcessManager),
                new BounceDirector(Engine, Content, ProcessManager),
                new LaserBeamCleanupDirector(Engine, Content, ProcessManager)
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
            Content.Load<Texture2D>(CVars.Get<string>("texture_particle_velocity"));

            Content.Load<SoundEffect>(CVars.Get<string>("sound_explosion"));
            Content.Load<SoundEffect>(CVars.Get<string>("sound_projectile_fired"));
            Content.Load<SoundEffect>(CVars.Get<string>("sound_projectile_bounce"));

            Content.Load<BitmapFont>(CVars.Get<string>("font_game_over"));

            Content.Load<Effect>(CVars.Get<string>("effect_blur"));

            Bloom bloom = new Bloom(AdriftPostProcessor, GameManager.Content);
            bloom.Radius = 1.5f;
            AdriftPostProcessor.Effects.Add(bloom);

            _fxaaPPE = new FXAA(AdriftPostProcessor, Content);
            AdriftPostProcessor.Effects.Add(_fxaaPPE);
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
                new Vector2(-25 + (25 * (PlayerArray.Length % 2)), 0));
            Entity playerShieldEntity = PlayerShieldEntity.Create(Engine,
                playerShipEntity);
            playerShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerShieldEntity;
            playerShieldEntity.AddComponent(new PlayerComponent(PlayerArray[0]));

            if (PlayerArray.Length == 2)
            {
                Entity playerTwoShipEntity = PlayerShipEntity.Create(Engine,
                new Vector2(25, 0));
                Entity playerTwoShieldEntity = PlayerShieldEntity.Create(Engine,
                    playerTwoShipEntity);
                playerTwoShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerTwoShieldEntity;
                playerTwoShieldEntity.AddComponent(new PlayerComponent(PlayerArray[1]));
            }

            Entity topEdge = EdgeEntity.Create(Engine, new Vector2(0, CVars.Get<int>("window_height") / 2), new Vector2(CVars.Get<int>("window_width"), 5), new Vector2(0,-1));
            Entity leftEdge = EdgeEntity.Create(Engine, new Vector2(-CVars.Get<int>("window_width") / 2, 0), new Vector2(5, CVars.Get<int>("window_height")), new Vector2(1, 0));
            Entity bottomEdge = EdgeEntity.Create(Engine, new Vector2(0, -CVars.Get<int>("window_height") / 2), new Vector2(CVars.Get<int>("window_width"), 5), new Vector2(0, 1));
            Entity rightEdge = EdgeEntity.Create(Engine, new Vector2(CVars.Get<int>("window_width") / 2, 0), new Vector2(5, CVars.Get<int>("window_height")), new Vector2(-1, 0));
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);

            _acculmulator += dt;
            while (_acculmulator >= 1 / CVars.Get<float>("tick_frequency"))
            {
                _acculmulator -= 1 / CVars.Get<float>("tick_frequency");

                for (int i = 0; i < _systems.Length; i++)
                {
                    _systems[i].Update(dt);
                }
            }

            VelocityParticleManager.Update(dt);
        }

        public override void Draw(float dt)
        {
            float betweenFrameAlpha = _acculmulator / (1 / CVars.Get<float>("tick_frequency"));

            _fxaaPPE.Enabled = CVars.Get<bool>("graphics_fxaa");

            AdriftPostProcessor.Begin();
            _renderSystem.DrawEntities(_mainCamera.TransformMatrix,
                                        Constants.Render.GROUP_MASK_ALL,
                                        dt,
                                        betweenFrameAlpha);
            _renderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                _mainCamera.TransformMatrix);
            VelocityParticleManager.Draw(_renderSystem.SpriteBatch);
            _renderSystem.SpriteBatch.End();
            AdriftPostProcessor.End();
        }

        public override void Dispose()
        {
            // Remove listeners
            EventManager.Instance.UnregisterListener(this);
            _mainCamera.UnregisterEvents();
            AdriftPostProcessor.UnregisterEvents();

            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].UnregisterEvents();
            }
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GameOverEvent)
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
                GameManager.ChangeState(new UIMenuGameState(GameManager));
            }));
        }
    }
}
