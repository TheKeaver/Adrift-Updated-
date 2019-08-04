using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Input;
using GameJam.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

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

        Player tmpPlayer; // This is temporary, it will be passed in from another game state later

        public MainGameState(GameManager gameManager)
            : base(gameManager)
        {
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            _mainCamera = new Camera(Constants.Global.WINDOW_WIDTH, Constants.Global.WINDOW_HEIGHT);
            _mainCamera.RegisterEvents();

            tmpPlayer = new Player("Keith", new PrimaryKeyboardInputMethod());

            InitSystems();
            InitDirectors();
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
                new KamikazeSystem(Engine)
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
                new SoundDirector(Engine, Content, ProcessManager)
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
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PLAYER_SHIP);
            Content.Load<Texture2D>(Constants.Resources.TEXTURE_PLAYER_SHIELD);
            Content.Load<SoundEffect>(Constants.Resources.SOUND_EXPLOSION);
            Content.Load<SoundEffect>(Constants.Resources.SOUND_LASER_FIRED);
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
                Content.Load<Texture2D>(Constants.Resources.TEXTURE_PLAYER_SHIP),
                new Vector2(0, 0));
            Entity playerShieldEntity = PlayerShieldEntity.Create(Engine,
                Content.Load<Texture2D>(Constants.Resources.TEXTURE_PLAYER_SHIELD), playerShipEntity);
            Entity kamikazeEntity = KamikazeEntity.Create(Engine,
                Content.Load<Texture2D>(Constants.Resources.TEXTURE_PLAYER_SHIP),
                new Vector2(150, 150));
            playerShieldEntity.AddComponent(new PlayerComponent(tmpPlayer));
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);

            _acculmulator += dt;
            while(_acculmulator >= Constants.Global.TICK_RATE)
            {
                _acculmulator -= Constants.Global.TICK_RATE;

                for(int i = 0; i < _systems.Length; i++)
                {
                    _systems[i].Update(dt);
                }
            }
        }

        public override void Draw(float dt)
        {
            float betweenFrameAlpha = _acculmulator / Constants.Global.TICK_RATE;

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
            return false;
        }
    }
}

