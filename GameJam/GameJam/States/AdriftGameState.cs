using Adrift.Content.Common.UI;
using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Events.Audio;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using GameJam.Graphics.Text;
using GameJam.Processes;
using GameJam.Processes.Animations;
using GameJam.Processes.Enemies;
using GameJam.Processes.Entities;
using GameJam.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            ResetPlayerInputMethods();

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
            _root.Render(_spriteBatch, _fieldFontRenderer); // UI

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

        private void ResetPlayerInputMethods()
        {
            foreach(Player player in Players)
            {
                player.InputMethod.Reset();
            }
        }

        private void CreateEntities()
        {
            // TODO: This should be passed in from the lobby, most likely just have the Player object contain the color
            // These would _normally_ be CVars, but this will be okay for now because it will give us an excuse to move
            // these to the lobby menu.
            Color[] colors = new Color[]
            {
                Color.White,
                Color.Blue,
                Color.Orange,
                Color.Magenta
            };

            for(int p = 0; p < Players.Length; p++)
            {
                SpawnPlayer(Players[p],
                    ComputePlayerShipSpawnPosition(p, Players.Length),
                    colors[p]);
            }

            EdgeEntity.Create(SharedState.Engine, new Vector2(0, CVars.Get<float>("play_field_height") / 2), new Vector2(CVars.Get<float>("play_field_width"), 5), new Vector2(0, -1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(-CVars.Get<float>("play_field_width") / 2, 0), new Vector2(5, CVars.Get<float>("play_field_height")), new Vector2(1, 0));
            EdgeEntity.Create(SharedState.Engine, new Vector2(0, -CVars.Get<float>("play_field_height") / 2), new Vector2(CVars.Get<float>("play_field_width"), 5), new Vector2(0, 1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(CVars.Get<float>("play_field_width") / 2, 0), new Vector2(5, CVars.Get<float>("play_field_height")), new Vector2(-1, 0));
        }

        private Vector2 ComputePlayerShipSpawnPosition(int playerIndex, int playerCount)
        {
            if(playerCount <= 1)
            {
                return Vector2.Zero;
            }

            float perPlayerAngle = MathHelper.TwoPi / playerCount;
            float playerAngle = playerIndex * perPlayerAngle;
            return CVars.Get<float>("player_ship_multiplayer_spawn_radius") * new Vector2((float)Math.Cos(playerAngle), (float)Math.Sin(playerAngle));
        }

        private void SpawnPlayer(Player player, Vector2 position, Color color)
        {
            Entity playerShipEntity = PlayerShipEntity.Create(SharedState.Engine, position, color);
            SuperShieldDisplayEntity.Create(SharedState.Engine, playerShipEntity);

            PlayerShipComponent playerShipComp = playerShipEntity.GetComponent<PlayerShipComponent>();
            // Defailt
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(0.0f), true));
            // Super shields
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(90.0f), false));
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(180.0f), false));
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(270.0f), false));

            playerShipEntity.AddComponent(new PlayerComponent(player));

            // Queue an event
            EventManager.Instance.QueueEvent(new PlayerShipSpawnEvent(playerShipEntity, position));
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

            {
                // Zoom out if the enemy is outside the camera (laser enemies)
                TransformComponent transformComp = responsibleEntity.GetComponent<TransformComponent>();
                BoundingRect boundRect = new BoundingRect();
                bool assumeOutside = false;
                if (responsibleEntity.HasComponent<VectorSpriteComponent>())
                {
                    boundRect = responsibleEntity.GetComponent<VectorSpriteComponent>().GetAABB(transformComp.Scale);
                } else if (responsibleEntity.HasComponent<SpriteComponent>())
                {
                    boundRect = responsibleEntity.GetComponent<SpriteComponent>().GetAABB(transformComp.Scale);
                } else
                {
                    assumeOutside = true;
                }
                boundRect.Min += transformComp.Position;
                boundRect.Max += transformComp.Position;

                if (!boundRect.Intersects(SharedState.Camera.BoundingRect) || assumeOutside)
                {
                    ProcessManager.Attach(new CameraPositionZoomResetProcess(SharedState.Camera, CVars.Get<float>("game_over_camera_reset_duration"), Vector2.Zero, 0.6f, Easings.Functions.CubicEaseOut));
                }
            }
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
            // TODO Why are these here instead of the PauseGameState? Something to note when moving them, TogglePauseAll will affect the PauseGameState so it needs to be unpausable (otherwise it will pause itself)
            GameManager.ProcessManager.TogglePauseAll();
            EventManager.Instance.QueueEvent(new PauseAllSoundsEvent());
            GameManager.ProcessManager.Attach(new PauseGameState(GameManager, SharedState, this));
        }
    }
}
