using System;
using Audrey;
using GameJam.Common;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Graphics;
using GameJam.Particles;
using GameJam.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.States
{
    public class SharedGameState : GameState
    {
        public PostProcessor PostProcessor
        {
            get;
            private set;
        }
        private FXAA _fxaaPPE;
        private SMAA _smaaPPE;

        public Camera Camera
        {
            get;
            private set;
        }
        public Camera UICamera
        {
            get;
            private set;
        }
#if DEBUG
        public Camera DebugCamera
        {
            get;
            private set;
        }
#endif

        public ParticleManager<VelocityParticleInfo> VelocityParticleManager
        {
            get;
            private set;
        }
        public GPUParticleManager GPUParticleManager {
            get;
            private set;
        }

        public Engine Engine
        {
            get;
            private set;
        }
        BaseSystem[] _systems;
        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }
        public RenderSystem RenderSystem
        {
            get;
            private set;
        }
#if DEBUG
        public CollisionDebugRenderSystem CollisionDebugRenderSystem
        {
            get;
            private set;
        }
#endif
#if DEBUG
        public RenderCullingDebugRenderSystem RenderCullingDebugRenderSystem
        {
            get;
            private set;
        }
#endif
#if DEBUG
        public QuadTreeDebugRenderSystem QuadTreeDebugRenderSystem
        {
            get;
            private set;
        }
#endif

        public SharedGameState(GameManager gameManager) : base(gameManager)
        {
        }

        protected override void OnInitialize()
        {
            SpriteBatch = new SpriteBatch(GameManager.GraphicsDevice);

            PostProcessor = new PostProcessor(GameManager.GraphicsDevice,
                CVars.Get<float>("screen_width"),
                CVars.Get<float>("screen_height"));
            PostProcessor.RegisterEvents(); // Responds to ResizeEvent; keep outside of RegisterListeners

            Camera = new Camera(CVars.Get<float>("screen_width"), CVars.Get<float>("screen_height"));
            Camera.RegisterEvents();

            UICamera = new Camera(CVars.Get<float>("screen_width"), CVars.Get<float>("screen_height"));
            UICamera.EnableCompensationZoom = false;
            UICamera.RegisterEvents();

            DebugCamera = new DebugCamera(CVars.Get<float>("screen_width"), CVars.Get<float>("screen_height"));
            DebugCamera.RegisterEvents();

            VelocityParticleManager = new ParticleManager<VelocityParticleInfo>(1024 * 20, VelocityParticleInfo.UpdateParticle);
            ProcessManager.Attach(VelocityParticleManager);
            GPUParticleManager = new GPUParticleManager(GameManager.GraphicsDevice,
                Content,
                "effect_gpu_particle_velocity");
            GPUParticleManager.RegisterListeners();

            Engine = new Engine();
            InitSystems();
            InitDirectors();

            LoadContent();

            CreateEntities();

            base.OnInitialize();
        }

        private void InitSystems()
        {
            _systems = new BaseSystem[]
            {
                // Input System must go first to have accurate snapshots
                new InputSystem(Engine),
                // TransformResetSystem must go before any system that changes the transform of entities
                new TransformResetSystem(Engine),

                // Section below is not dependent on other systems
                new GravityHolePassiveAnimationSystem(Engine, ProcessManager, Content),
                new AnimationSystem(Engine),
                new ParallaxBackgroundSystem(Engine, Camera),
                new PulseSystem(Engine),
                new PassiveRotationSystem(Engine),
                new MenuBackgroundDestructionSystem(Engine, Camera),
                new ProjectileColorSyncSystem(Engine),

                // Section below is ordered based on dependency from Top (least dependent) to Bottom (most dependent)
                new ChasingSpeedIncreaseSystem(Engine),
                new LaserEnemySystem(Engine),
                new GravitySystem(Engine),
                new EnemySeparationSystem(Engine), // Depends on the Quad Tree, however this system needs to go before movement system. It will use the previous frame's quad tree.
                new MovementSystem(Engine),
                new PlayerShieldSystem(Engine),
                new QuadTreeSystem(Engine),
                new SuperShieldSystem(Engine),
                
                // Until Changed, EnemyRotationSystem must go after MovementSystem or enemy ships will not bounce off of walls
                new EnemyRotationSystem(Engine),
                new EntityMirroringSystem(Engine),

                // Collision Detection must go last to have accurate collision detection
                new CollisionDetectionSystem(Engine)
            };

            RenderSystem = new RenderSystem(GameManager.GraphicsDevice, Content, Engine);
#if DEBUG
            CollisionDebugRenderSystem = new CollisionDebugRenderSystem(GameManager.GraphicsDevice, Engine);
#endif
#if DEBUG
            RenderCullingDebugRenderSystem = new RenderCullingDebugRenderSystem(GameManager.GraphicsDevice, Engine);
#endif
#if DEBUG
            QuadTreeDebugRenderSystem = new QuadTreeDebugRenderSystem(GameManager.GraphicsDevice, Engine);
#endif
        }
        private void InitDirectors()
        {
            ProcessManager.Attach(new EnemyCollisionWithShipDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new EnemyCollisionOnShieldDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new SoundDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new ExplosionDirector(Engine, Content, ProcessManager, VelocityParticleManager, GPUParticleManager));

            ProcessManager.Attach(new ChangeToChasingEnemyDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new EnemyPushBackOnPlayerDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new HazardCollisionOnEnemyDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new BounceDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new LaserBeamCleanupDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new PlayerShipCollidingWithEdgeDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new GameOverDeciderDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new SuperShieldDisplayCleanupDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new CleanupEntitiesOnEndOfSceneDirector(Engine, Content, ProcessManager));

            ProcessManager.Attach(new PlayerLostAnimationAttachDirector(Engine, Content, ProcessManager));
        }

        private void LoadContent()
        {
            Bloom bloom = new Bloom(PostProcessor, Content);
            bloom.Radius = 1.5f;
            PostProcessor.Effects.Add(bloom);

            _fxaaPPE = new FXAA(PostProcessor, Content);
            PostProcessor.Effects.Add(_fxaaPPE);

            //Negative negative = new Negative(PostProcessor, Content);
            //PostProcessor.Effects.Add(negative);

            _smaaPPE = new SMAA(PostProcessor, Content);
            PostProcessor.Effects.Add(_smaaPPE);
        }

        private void CreateEntities()
        {
            CreateParallaxBackground();
        }

        void CreateParallaxBackground()
        {
            ParallaxBackgroundEntity.Create(Engine,
                new TextureRegion2D(Content.Load<Texture2D>("texture_background_stars_0")),
                Vector2.Zero, 0.15f, true);
            ParallaxBackgroundEntity.Create(Engine,
                new TextureRegion2D(Content.Load<Texture2D>("texture_background_stars_1")),
                Vector2.Zero, 0.25f);
            ParallaxBackgroundEntity.Create(Engine,
                new TextureRegion2D(Content.Load<Texture2D>("texture_background_stars_2")),
                Vector2.Zero, 0.35f);
            ParallaxBackgroundEntity.Create(Engine,
                new TextureRegion2D(Content.Load<Texture2D>("texture_background_stars_3")),
                Vector2.Zero, 0.55f);

            //ParallaxBackgroundEntity.Create(Engine,
            //    Content.Load<TextureRegion2D>("texture_background_parallax_test"),
            //    Vector2.Zero, 0.55f);
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            Camera.ResetAll();
            DebugCamera.ResetAll();

            for (int i = 0; i < _systems.Length; i++)
            {
                _systems[i].Update(dt);
            }

            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            int enableFrameSmoothing = CVars.Get<bool>("graphics_frame_smoothing") ? 1 : 0;
            betweenFrameAlpha = betweenFrameAlpha * enableFrameSmoothing + (1 - enableFrameSmoothing);

            _fxaaPPE.Enabled = CVars.Get<bool>("graphics_fxaa");
            _smaaPPE.Enabled = CVars.Get<bool>("graphics_smaa");

            GameManager.GraphicsDevice.Clear(Color.Black);

            Camera camera = Camera;
#if DEBUG
            if(CVars.Get<bool>("debug_force_debug_camera"))
            {
                camera = DebugCamera;
            }
#endif

            PostProcessor.Begin();
            {
                RenderSystem.DrawEntities(Camera,
                                            Constants.Render.RENDER_GROUP_GAME_ENTITIES,
                                            dt,
                                            betweenFrameAlpha,
                                            camera);
                //RenderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                //    null,
                //    null,
                //    null,
                //    null,
                //    null,
                //    camera.GetInterpolatedTransformMatrix(betweenFrameAlpha));
                //if (!CVars.Get<bool>("particle_gpu_accelerated"))
                //{
                //    VelocityParticleManager.Draw(RenderSystem.SpriteBatch);
                //}
                //RenderSystem.SpriteBatch.End();
                if (CVars.Get<bool>("particle_gpu_accelerated"))
                {
                    GPUParticleManager.UpdateAndDraw(Camera, dt, betweenFrameAlpha, camera);
                }
            }
            // We have to defer drawing the post-processor results
            // because of unexpected behavior within MonoGame.
            RenderTarget2D postProcessingResult = PostProcessor.End(false);

            // Stars
            RenderSystem.DrawEntities(Camera,
                                        Constants.Render.RENDER_GROUP_STARS,
                                        dt,
                                        betweenFrameAlpha, camera); // Stars
            SpriteBatch.Begin();
            SpriteBatch.Draw(postProcessingResult,
                postProcessingResult.Bounds,
                Color.White); // Post-processing results
            SpriteBatch.End();

            RenderSystem.DrawEntities(UICamera, Constants.Render.RENDER_GROUP_UI, dt, betweenFrameAlpha);

            // Shield Resource
            RenderSystem.DrawEntities(Camera,
                                      Constants.Render.RENDER_GROUP_NO_GLOW,
                                      dt,
                                      betweenFrameAlpha, camera);

#if DEBUG
            if (CVars.Get<bool>("debug_show_collision_shapes"))
            {
                CollisionDebugRenderSystem.Draw(camera.GetInterpolatedTransformMatrix(betweenFrameAlpha), dt);
            }
#endif
#if DEBUG
            if (CVars.Get<bool>("debug_show_render_culling"))
            {
                Camera debugCamera = CVars.Get<bool>("debug_force_debug_camera") ? DebugCamera : null;
                RenderCullingDebugRenderSystem.Draw(Camera, dt, debugCamera);
            }
#endif
#if DEBUG
            if (CVars.Get<bool>("debug_show_quad_trees"))
            {
                Camera debugCamera = CVars.Get<bool>("debug_force_debug_camera") ? DebugCamera : null;
                QuadTreeDebugRenderSystem.Draw(Camera, dt, debugCamera);
            }
#endif

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            PostProcessor.UnregisterEvents();
            Camera.UnregisterEvents();
            UICamera.UnregisterEvents();
            GPUParticleManager.UnregisterListeners();

            throw new Exception("This game state provides shared logic with all other game states and must not be killed.");
        }
    }
}
