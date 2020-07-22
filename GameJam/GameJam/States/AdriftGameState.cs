using Adrift.Content.Common.UI;
using Audrey;
using Events;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Events.Animation;
using GameJam.Events.Audio;
using GameJam.Events.GameLogic;
using GameJam.Graphics.Text;
using GameJam.Processes;
using GameJam.Processes.Animations;
using GameJam.Processes.Enemies;
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

        private bool ArmedForSceneChangeOnPlayerLostAnimationComplete = false; // TODO This doesn't have logic to switch scenes yet

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
            ProcessManager.Attach(new SpawnPatternManager(SharedState.Engine, ProcessManager, SharedState.World));

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
                // This is a command, it can be triggered instead of queued
                EventManager.Instance.TriggerEvent(new CleanupAllEntitiesEvent());
            }

            base.OnKill();
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<GameOverEvent>(this);
            EventManager.Instance.RegisterListener<TogglePauseGameEvent>(this);
            EventManager.Instance.RegisterListener<PlayerLostAnimationCompletedEvent>(this);
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
            //PlayerTrailEntity.Create(SharedState.Engine, playerShipEntity);

            PlayerShipComponent playerShipComp = playerShipEntity.GetComponent<PlayerShipComponent>();
            // Default
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(0.0f), true));
            // Super shields
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(90.0f), false));
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(180.0f), false));
            playerShipComp.AddShield(PlayerShieldEntity.Create(SharedState.Engine, playerShipEntity, MathHelper.ToRadians(270.0f), false));

            playerShipEntity.AddComponent(new PlayerComponent(player));

            // Create the SuperShieldDisplayEntity and pass in the shipEntity
            //SuperShieldDisplayEntity.Create(SharedState.Engine, playerShipEntity);

            // Create the VectorSpriteTrailEntity and pass in the shipEntity
            VectorSpriteTrailEntity.Create(SharedState.Engine, playerShipEntity);

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
            if(evt is PlayerLostAnimationCompletedEvent)
            {
                HandlePlayerLostAnimationCompletedEvent(evt as PlayerLostAnimationCompletedEvent);
            }

            return false;
        }

        private void HandleGameOverEvent(GameOverEvent gameOverEvent)
        {
            // Kill all processes except Camera process, because Camera process
            // is used by the player lost animation
            foreach(Process process in ProcessManager.Processes)
            {
                if(!(process is CameraProcess))
                {
                    process.Kill();
                }
            }

            ArmedForSceneChangeOnPlayerLostAnimationComplete = true;
        }

        private void HandlePause()
        {
            // TODO Why are these here instead of the PauseGameState? Something to note when moving them, TogglePauseAll will affect the PauseGameState so it needs to be unpausable (otherwise it will pause itself)
            GameManager.ProcessManager.TogglePauseAll();
            EventManager.Instance.QueueEvent(new PauseAllSoundsEvent());
            GameManager.ProcessManager.Attach(new PauseGameState(GameManager, SharedState, this));
        }

        private void HandlePlayerLostAnimationCompletedEvent(PlayerLostAnimationCompletedEvent playerLostAnimationCompletedEvent)
        {
            if(ArmedForSceneChangeOnPlayerLostAnimationComplete)
            {
                // This is a "command", trigger instead of queue
                EventManager.Instance.TriggerEvent(new CleanupAllEntitiesEvent());
                // Move camera towards center of screen (owned by shared state)
                SharedState.ProcessManager.Attach(new CameraPositionZoomResetProcess(SharedState.Camera, CVars.Get<float>("game_over_camera_reset_duration"), Vector2.Zero, 1, Easings.Functions.CubicEaseOut));
                ChangeState(new GameOverGameState(GameManager, SharedState, Players, playerScoreDirector.GetScores()));
            }
        }
    }
}
