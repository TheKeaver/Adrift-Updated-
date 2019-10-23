﻿using System;
using Audrey;
using GameJam.Common;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Graphics;
using GameJam.Particles;
using GameJam.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class SharedGameState : GameState
    {
        public ProcessManager ProcessManager
        {
            get;
            private set;
        }

        public PostProcessor PostProcessor
        {
            get;
            private set;
        }
        private FXAA _fxaaPPE;

        public Camera Camera
        {
            get;
            private set;
        }

        public ParticleManager<VelocityParticleInfo> VelocityParticleManager
        {
            get;
            private set;
        }

        public Engine Engine
        {
            get;
            private set;
        }
        BaseSystem[] _systems;
        public RenderSystem RenderSystem
        {
            get;
            private set;
        }
        BaseDirector[] _directors;

        public SharedGameState(GameManager gameManager) : base(gameManager)
        {
        }

        protected override void OnInitialize()
        {
            ProcessManager = new ProcessManager();

            PostProcessor = new PostProcessor(GameManager.GraphicsDevice,
                CVars.Get<int>("window_width"),
                CVars.Get<int>("window_height"));
            PostProcessor.RegisterEvents();

            Camera = new Camera(CVars.Get<int>("window_width"), CVars.Get<int>("window_height"));
            Camera.RegisterEvents();

            VelocityParticleManager = new ParticleManager<VelocityParticleInfo>(1024 * 20, VelocityParticleInfo.UpdateParticle);
            ProcessManager.Attach(VelocityParticleManager);

            Engine = new Engine();
            InitSystems();
            InitDirectors();

            LoadContent();

            CreateEntities();

            RegisterListeners();

            base.OnInitialize();
        }

        private void InitSystems()
        {
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
                new ParallaxBackgroundSystem(Engine, Camera),
                new PassiveRotationSystem(Engine),
                new MenuBackgroundDestructionSystem(Engine)
            };

            RenderSystem = new RenderSystem(GameManager.GraphicsDevice, Engine);
        }
        private void InitDirectors()
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

        private void LoadContent()
        {
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

            Content.Load<Effect>(CVars.Get<string>("effect_blur"));
            Bloom bloom = new Bloom(PostProcessor, GameManager.Content);
            bloom.Radius = 1.5f;
            PostProcessor.Effects.Add(bloom);

            _fxaaPPE = new FXAA(PostProcessor, Content);
            PostProcessor.Effects.Add(_fxaaPPE);
        }

        private void CreateEntities()
        {
            CreateParallaxBackground();
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

        private void RegisterListeners()
        {
        }
        private void UnregisterListeners()
        {
        }

        protected override void OnUpdate(float dt)
        {
            ProcessManager.Update(dt);

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

            PostProcessor.Begin();
            {
                RenderSystem.DrawEntities(Camera.TransformMatrix,
                                            Constants.Render.RENDER_GROUP_GAME_ENTITIES,
                                            dt,
                                            betweenFrameAlpha);
                RenderSystem.SpriteBatch.Begin(SpriteSortMode.Deferred,
                    null,
                    null,
                    null,
                    null,
                    null,
                    Camera.TransformMatrix);
                VelocityParticleManager.Draw(RenderSystem.SpriteBatch);
                RenderSystem.SpriteBatch.End();
            }
            // We have to defer drawing the post-processor results
            // because of unexpected behavior within MonoGame.
            RenderTarget2D postProcessingResult = PostProcessor.End(false);

            // Stars
            RenderSystem.DrawEntities(Camera.TransformMatrix,
                                        Constants.Render.RENDER_GROUP_STARS,
                                        dt,
                                        betweenFrameAlpha); // Stars
            RenderSystem.SpriteBatch.Begin();
            RenderSystem.SpriteBatch.Draw(postProcessingResult,
                postProcessingResult.Bounds,
                Color.White); // Post-processing results
            RenderSystem.SpriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            UnregisterListeners();
            PostProcessor.UnregisterEvents();
            Camera.UnregisterEvents();

            for (int i = 0; i < _directors.Length; i++)
            {
                _directors[i].UnregisterEvents();
            }

            throw new Exception("This game state provides shared logic with all other game states and must not be killed.");
        }
    }
}