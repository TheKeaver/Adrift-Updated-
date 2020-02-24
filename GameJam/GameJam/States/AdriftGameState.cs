using Adrift.Content.Common.UI;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Events.Audio;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using GameJam.Graphics.Text;
using GameJam.Processes;
using GameJam.Processes.Animation;
using GameJam.Processes.Animations;
using GameJam.Processes.Enemies;
using GameJam.Processes.Entities;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;

namespace GameJam.States
{
    /// <summary>
    /// Game state for the main game.
    /// </summary>
    public class AdriftGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        FieldFontRenderer _fieldFontRenderer;
        Root _root;

        public int score;

        private bool _entitiesCleanedUp = false;

        public Player[] Players
        {
            get;
            private set;
        }

        private PlayerScoreDirector playerScoreDirector;

        public AdriftGameState(GameManager gameManager,
            SharedGameState sharedState,
            Player[] players)
            : base(gameManager, sharedState)
        {
            Players = players;
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            //ProcessManager.Attach(new ChasingEnemySpawner(SharedState.Engine, ProcessManager));
            //ProcessManager.Attach(new ShooterEnemySpawner(SharedState.Engine, ProcessManager));
            //ProcessManager.Attach(new GravityEnemySpawner(SharedState.Engine, ProcessManager));
            //ProcessManager.Attach(new LaserEnemySpawner(SharedState.Engine, ProcessManager));
            ProcessManager.Attach(new PauseDirector(SharedState.Engine, Content, ProcessManager));
            ProcessManager.Attach(new CameraProcess(SharedState.Camera, SharedState.Engine));
            ProcessManager.Attach(new SpawnPatternManager(SharedState.Engine, ProcessManager));

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
            _root.AutoControlModeSwitching = false;

            LoadContent();

            playerScoreDirector = new PlayerScoreDirector(Players, _root, SharedState.Engine, Content, ProcessManager);
            ProcessManager.Attach(playerScoreDirector);

            CreateEntities();

            base.OnInitialize();
        }

        private void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_adrift_game_ui"));
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _fieldFontRenderer.Begin();
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch, _fieldFontRenderer); // UI
            _spriteBatch.End();
            _fieldFontRenderer.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            if (!_entitiesCleanedUp)
            {
                CleanDestroyAllEntities();
            }

            base.OnKill();
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<GameOverEvent>(this);
            EventManager.Instance.RegisterListener<TogglePauseGameEvent>(this);
        }

        protected override void UnregisterListeners()
        {
            _root.UnregisterListeners();
            
            EventManager.Instance.UnregisterListener(this);
        }

        void CreateEntities()
        {
            Entity playerShipEntity = PlayerShipEntity.Create(SharedState.Engine,
                new Vector2((-25 * (Players.Length / 2)) + (25 * (Players.Length / 4)), 25 * (Players.Length / 3)), Color.White);

            Entity playerShieldEntityDefault = PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(0.0f), true);
            Entity playerShieldEntity90 = PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(90.0f), false);
            Entity playerShieldEntity180 = PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(180.0f), false);
            Entity playerShieldEntity270 = PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(270.0f), false);

            playerShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerShieldEntityDefault);
            playerShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerShieldEntity90);
            playerShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerShieldEntity180);
            playerShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerShieldEntity270);

            playerShieldEntityDefault.AddComponent(new PlayerComponent(Players[0]));
            playerShieldEntity90.AddComponent(new PlayerComponent(Players[0]));
            playerShieldEntity180.AddComponent(new PlayerComponent(Players[0]));
            playerShieldEntity270.AddComponent(new PlayerComponent(Players[0]));
            Players[0].InputMethod.GetSnapshot().Angle = 0;

            if (Players.Length >= 2)
            {
                Entity playerTwoShipEntity = PlayerShipEntity.Create(SharedState.Engine,
                new Vector2(25, 25 * (Players.Length / 3)), Color.Blue);

                Entity playerTwoShieldEntityDefault = PlayerShieldEntity.Create(SharedState.Engine, playerTwoShipEntity, MathHelper.ToRadians(0.0f), true);
                Entity playerTwoShieldEntity90 = PlayerShieldEntity.Create(SharedState.Engine, playerTwoShipEntity, MathHelper.ToRadians(90.0f), false);
                Entity playerTwoShieldEntity180 = PlayerShieldEntity.Create(SharedState.Engine, playerTwoShipEntity, MathHelper.ToRadians(180.0f), false);
                Entity playerTwoShieldEntity270 = PlayerShieldEntity.Create(SharedState.Engine, playerTwoShipEntity, MathHelper.ToRadians(270.0f), false);

                playerTwoShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerTwoShieldEntityDefault);
                playerTwoShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerTwoShieldEntity90);
                playerTwoShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerTwoShieldEntity180);
                playerTwoShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerTwoShieldEntity270);

                playerTwoShieldEntityDefault.AddComponent(new PlayerComponent(Players[1]));
                playerTwoShieldEntity90.AddComponent(new PlayerComponent(Players[1]));
                playerTwoShieldEntity180.AddComponent(new PlayerComponent(Players[1]));
                playerTwoShieldEntity270.AddComponent(new PlayerComponent(Players[1]));

                Players[1].InputMethod.GetSnapshot().Angle = 0;
            }

            if( Players.Length >= 3)
            {
                Entity playerThreeShipEntity = PlayerShipEntity.Create(SharedState.Engine, new Vector2(-25, -25), Color.Orange);

                Entity playerThreeShieldEntityDefault = PlayerShieldEntity.Create(SharedState.Engine, playerThreeShipEntity, MathHelper.ToRadians(0.0f), true);
                Entity playerThreeShieldEntity90 = PlayerShieldEntity.Create(SharedState.Engine, playerThreeShipEntity, MathHelper.ToRadians(90.0f), false);
                Entity playerThreeShieldEntity180 = PlayerShieldEntity.Create(SharedState.Engine, playerThreeShipEntity, MathHelper.ToRadians(180.0f), false);
                Entity playerThreeShieldEntity270 = PlayerShieldEntity.Create(SharedState.Engine, playerThreeShipEntity, MathHelper.ToRadians(270.0f), false);

                playerThreeShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerThreeShieldEntityDefault);
                playerThreeShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerThreeShieldEntity90);
                playerThreeShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerThreeShieldEntity180);
                playerThreeShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerThreeShieldEntity270);

                playerThreeShieldEntityDefault.AddComponent(new PlayerComponent(Players[2]));
                playerThreeShieldEntity90.AddComponent(new PlayerComponent(Players[2]));
                playerThreeShieldEntity180.AddComponent(new PlayerComponent(Players[2]));
                playerThreeShieldEntity270.AddComponent(new PlayerComponent(Players[2]));

                Players[2].InputMethod.GetSnapshot().Angle = 0;
            }

            if( Players.Length == 4)
            {
                Entity playerFourShipEntity = PlayerShipEntity.Create(SharedState.Engine, new Vector2(25, -25), Color.Magenta);

                Entity playerFourShieldEntityDefault = PlayerShieldEntity.Create(SharedState.Engine, playerFourShipEntity, MathHelper.ToRadians(0.0f), true);
                Entity playerFourShieldEntity90 = PlayerShieldEntity.Create(SharedState.Engine, playerFourShipEntity, MathHelper.ToRadians(90.0f), false);
                Entity playerFourShieldEntity190 = PlayerShieldEntity.Create(SharedState.Engine, playerFourShipEntity, MathHelper.ToRadians(180.0f), false);
                Entity playerFourShieldEntity270 = PlayerShieldEntity.Create(SharedState.Engine, playerFourShipEntity, MathHelper.ToRadians(270.0f), false);

                playerFourShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerFourShieldEntityDefault);
                playerFourShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerFourShieldEntity90);
                playerFourShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerFourShieldEntity190);
                playerFourShipEntity.GetComponent<PlayerShipComponent>().AddShield(playerFourShieldEntity270);

                playerFourShieldEntityDefault.AddComponent(new PlayerComponent(Players[3]));
                playerFourShieldEntity90.AddComponent(new PlayerComponent(Players[3]));
                playerFourShieldEntity190.AddComponent(new PlayerComponent(Players[3]));
                playerFourShieldEntity270.AddComponent(new PlayerComponent(Players[3]));

                Players[3].InputMethod.GetSnapshot().Angle = 0;
            }

            EdgeEntity.Create(SharedState.Engine, new Vector2(0, CVars.Get<float>("play_field_height") / 2), new Vector2(CVars.Get<float>("play_field_width"), 5), new Vector2(0, -1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(-CVars.Get<float>("play_field_width") / 2, 0), new Vector2(5, CVars.Get<float>("play_field_height")), new Vector2(1, 0));
            EdgeEntity.Create(SharedState.Engine, new Vector2(0, -CVars.Get<float>("play_field_height") / 2), new Vector2(CVars.Get<float>("play_field_width"), 5), new Vector2(0, 1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(CVars.Get<float>("play_field_width") / 2, 0), new Vector2(5, CVars.Get<float>("play_field_height")), new Vector2(-1, 0));
        }

        public bool Handle(IEvent evt)
        {
            if (evt is GameOverEvent)
            {
                HandleGameOverEvent(evt as GameOverEvent);
            }
            if(evt is TogglePauseGameEvent)
            {
                HandlePause();
                return true;
            }

            return false;
        }

        private void CleanDestroyAllEntities(bool includeEdges = true)
        {
            // Explode all entities
            ImmutableList<Entity> explosionEntities = SharedState.Engine.GetEntitiesFor(Family
                .All(typeof(TransformComponent), typeof(ColoredExplosionComponent))
                .Exclude(typeof(DontDestroyForGameOverComponent))
                .Get());
            foreach (Entity entity in explosionEntities)
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();
                ColoredExplosionComponent coloredExplosionComp = entity.GetComponent<ColoredExplosionComponent>();
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(transformComp.Position,
                    coloredExplosionComp.Color,
                    false));
            }

            // Destroy all entities
            FamilyBuilder familyBuilder = Family.Exclude(typeof(DontDestroyForGameOverComponent),
                typeof(ParallaxBackgroundComponent));
            if (!includeEdges)
            {
                familyBuilder.Exclude(typeof(EdgeComponent));
            }
            SharedState.Engine.DestroyEntitiesFor(familyBuilder.Get());
        }

        private void HandleGameOverEvent(GameOverEvent gameOverEvent)
        {
            ProcessManager.KillAll();
            Entity responsibleEntity = gameOverEvent.ResponsibleEntity;
            responsibleEntity.AddComponent(new DontDestroyForGameOverComponent());
            ImmutableList<IComponent> components = responsibleEntity.GetComponents();
            if (responsibleEntity.HasComponent<ProjectileComponent>())
            {
                responsibleEntity.AddComponent(new ColoredExplosionComponent(responsibleEntity.GetComponent<ProjectileComponent>().Color));
            }
            for (int i = components.Count - 1; i >= 0; i--)
            {
                if (!(components[i] is TransformComponent)
                    && !(components[i] is VectorSpriteComponent)
                    && !(components[i] is ColoredExplosionComponent)
                    && !(components[i] is DontDestroyForGameOverComponent))
                {
                    responsibleEntity.RemoveComponent(components[i].GetType());
                }
            }

            CleanDestroyAllEntities(false);
            _entitiesCleanedUp = true;

            ProcessManager.Attach(new CameraPositionZoomResetProcess(SharedState.Camera, CVars.Get<float>("game_over_camera_reset_duration"), Vector2.Zero, 0.6f, Easings.Functions.CubicEaseOut));
            ProcessManager.Attach(new SpriteEntityFlashProcess(SharedState.Engine, responsibleEntity, CVars.Get<int>("game_over_responsible_enemy_flash_count"), CVars.Get<float>("game_over_responsible_enemy_flash_period") / 2))
                .SetNext(new DelegateProcess(() =>
            {
                // Explosion
                TransformComponent transformComp = responsibleEntity.GetComponent<TransformComponent>();
                ColoredExplosionComponent coloredExplosionComp = responsibleEntity.GetComponent<ColoredExplosionComponent>();
                EventManager.Instance.QueueEvent(new CreateExplosionEvent(transformComp.Position,
                    coloredExplosionComp.Color,
                    false));

                // Fade out edges (owned by shared state)
                foreach (Entity edgeEntity in SharedState.Engine.GetEntitiesFor(Family.All(typeof(EdgeComponent), typeof(VectorSpriteComponent)).Get()))
                {
                    SharedState.ProcessManager.Attach(new SpriteEntityFadeOutProcess(SharedState.Engine, edgeEntity, CVars.Get<float>("game_over_edge_fade_out_duration"), Easings.Functions.QuadraticEaseOut))
                        .SetNext(new EntityDestructionProcess(SharedState.Engine, edgeEntity));
                }

                // Move camera towards center of screen (owned by shared state)
                SharedState.ProcessManager.Attach(new CameraPositionZoomResetProcess(SharedState.Camera, CVars.Get<float>("game_over_camera_reset_duration"), Vector2.Zero, 1, Easings.Functions.CubicEaseOut));
            })).SetNext(new EntityDestructionProcess(SharedState.Engine, responsibleEntity)).SetNext(new DelegateProcess(() =>
            {
                ChangeState(new GameOverGameState(GameManager, SharedState, Players, playerScoreDirector.GetScores()));
            }));
        }

        private void HandlePause()
        {
            GameManager.ProcessManager.TogglePauseAll();
            EventManager.Instance.QueueEvent(new PauseAllSoundsEvent());
            GameManager.ProcessManager.Attach(new PauseGameState(GameManager, SharedState, this));
        }
    }
}
