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
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System.Collections.Generic;
using UI.Content.Pipeline;

namespace GameJam.States
{
    /// <summary>
    /// Game state for the main game.
    /// </summary>
    public class MainGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        public int score;

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
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
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

            ProcessManager.Attach(new KamikazeSpawner(Engine));
            ProcessManager.Attach(new ShooterEnemySpawner(Engine, ProcessManager));
            ProcessManager.Attach(new GravityEnemySpawner(Engine, ProcessManager));
            ProcessManager.Attach(new LaserEnemySpawner(Engine, ProcessManager));

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);

            LoadContent();

            CreateEntities();

            _root.RegisterListeners();
            RegisterEvents();

            // Updates the camera and post-processor with the actual screen size
            // This fixes a bug present in a build of Super Pong
            // Must be triggered after all listeners are registered
            EventManager.Instance.TriggerEvent(new ResizeEvent(GameManager.GraphicsDevice.Viewport.Width,
                                                              GameManager.GraphicsDevice.Viewport.Height));
            base.OnInitialize();
        }

        private void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/MainGameStateUI"));

            Content.Load<Texture2D>(CVars.Get<string>("texture_particle_velocity"));

            Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_0"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_1"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_2"));
            Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_3"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_4"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_5"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_6"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_7"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_8"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_9"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_10"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_11"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_12"));
            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_13"));

            //Content.Load<Texture2D>(CVars.Get<string>("texture_background_parallax_test"));

            Content.Load<Texture2D>(CVars.Get<string>("texture_title_without_instructions"));

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

        protected override void OnUpdate(float dt)
        {
            ProcessManager.Update(dt);
            VelocityParticleManager.Update(dt);

            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            for (int i = 0; i < _systems.Length; i++)
            {
                _systems[i].Update(dt);
            }

            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _fxaaPPE.Enabled = CVars.Get<bool>("graphics_fxaa");

            GameManager.GraphicsDevice.Clear(Color.Black);

            AdriftPostProcessor.Begin();
            {
                _renderSystem.DrawEntities(_mainCamera.TransformMatrix,
                                            Constants.Render.RENDER_GROUP_GAME_ENTITIES,
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
            }
            // We have to defer drawing the post-processor results
            // because of unexpected behavior within MonoGame.
            RenderTarget2D postProcessing = AdriftPostProcessor.End(false);

            // Stars
            _renderSystem.DrawEntities(_mainCamera.TransformMatrix,
                                        Constants.Render.RENDER_GROUP_STARS,
                                        dt,
                                        betweenFrameAlpha); // Stars
            _spriteBatch.Begin();
            _spriteBatch.Draw(postProcessing,
                postProcessing.Bounds,
                Color.White); // Post-processing results
            _root.Draw(_spriteBatch); // UI
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            // Remove listeners
            UnregisterEvents();
            _root.UnregisterListeners();
            _mainCamera.UnregisterEvents();
            AdriftPostProcessor.UnregisterEvents();

            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].UnregisterEvents();
            }

            base.OnKill();
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<IncreasePlayerScoreEvent>(this);
            EventManager.Instance.RegisterListener<GameOverEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
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
                new EnemyRotationSystem(Engine),
                new AnimationSystem(Engine),
                new LaserEnemySystem(Engine),
                new GravitySystem(Engine),
                new PulseSystem(Engine),
                new ParallaxBackgroundSystem(Engine, _mainCamera)
            };

            _renderSystem = new RenderSystem(GameManager.GraphicsDevice, Engine);
        }
        void InitDirectors()
        {
            _directors = new BaseDirector[]
            {
                new ShipDirector(Engine, Content, ProcessManager),
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

        void CreateEntities()
        {
            CreateParallaxBackground();

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

            EdgeEntity.Create(Engine, new Vector2(0, CVars.Get<int>("window_height") / 2), new Vector2(CVars.Get<int>("window_width"), 5), new Vector2(0, -1));
            EdgeEntity.Create(Engine, new Vector2(-CVars.Get<int>("window_width") / 2, 0), new Vector2(5, CVars.Get<int>("window_height")), new Vector2(1, 0));
            EdgeEntity.Create(Engine, new Vector2(0, -CVars.Get<int>("window_height") / 2), new Vector2(CVars.Get<int>("window_width"), 5), new Vector2(0, 1));
            EdgeEntity.Create(Engine, new Vector2(CVars.Get<int>("window_width") / 2, 0), new Vector2(5, CVars.Get<int>("window_height")), new Vector2(-1, 0));
        }

        void CreateParallaxBackground()
        {
            ParallaxBackgroundEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_0")),
                Vector2.Zero, 0.15f, true);
            ParallaxBackgroundEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_1")),
                Vector2.Zero, 0.25f);
            ParallaxBackgroundEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_2")),
                Vector2.Zero, 0.35f);
            ParallaxBackgroundEntity.Create(Engine,
                Content.Load<Texture2D>(CVars.Get<string>("texture_background_stars_3")),
                Vector2.Zero, 0.55f);
            //ParallaxBackgroundEntity.Create(Engine,
            //    Content.Load<Texture2D>(CVars.Get<string>("texture_background_parallax_test")),
            //    Vector2.Zero, 0.55f);
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GameOverEvent)
            {
                HandleGameOverEvent(evt as GameOverEvent);
            }
            if (evt is IncreasePlayerScoreEvent)
            {
                HandleIncreasePlayerScoreEvent(evt as IncreasePlayerScoreEvent);
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
                ChangeState(new UIMenuGameState(GameManager));
            }));
        }

        private void HandleIncreasePlayerScoreEvent(IncreasePlayerScoreEvent increasePlayerScoreEvent)
        {
            score += increasePlayerScoreEvent.ScoreAddend;
            ((Label)_root.FindWidgetByID("main_game_score_label")).Content = "Score: " + score;
        }
    }
}
