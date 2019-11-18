using Audrey;
using Events;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Entities;
using GameJam.Events;
using GameJam.Events.EnemyActions;
using GameJam.Events.GameLogic;
using GameJam.Events.UI.Pause;
using GameJam.Processes;
using GameJam.Processes.Animation;
using GameJam.Processes.Enemies;
using GameJam.Processes.Entities;
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
    public class AdriftGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        public int score;

        Player[] PlayerArray;

        public AdriftGameState(GameManager gameManager,
            SharedGameState sharedState,
            Player[] players)
            : base(gameManager, sharedState)
        {
            PlayerArray = players;
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            ProcessManager.Attach(new KamikazeSpawner(SharedState.Engine));
            ProcessManager.Attach(new ShooterEnemySpawner(SharedState.Engine, ProcessManager));
            ProcessManager.Attach(new GravityEnemySpawner(SharedState.Engine, ProcessManager));
            ProcessManager.Attach(new LaserEnemySpawner(SharedState.Engine, ProcessManager));
            ProcessManager.Attach(new PauseDirector(SharedState.Engine, Content, ProcessManager));

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);

            LoadContent();

            CreateEntities();

            base.OnInitialize();
        }

        private void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_adrift_game_ui"));

            Content.Load<Texture2D>("texture_title_without_instructions");

            Content.Load<SoundEffect>("sound_explosion");
            Content.Load<SoundEffect>("sound_projectile_fired");
            Content.Load<SoundEffect>("sound_projectile_bounce");

            Content.Load<BitmapFont>("font_game_over");
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch); // UI
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            CleanDestroyAllEntities();

            base.OnKill();
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<IncreasePlayerScoreEvent>(this);
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
                new Vector2((-25 * (PlayerArray.Length / 2)) + (25 * (PlayerArray.Length / 4)), 25 * (PlayerArray.Length / 3)));
            Entity playerShieldEntity = PlayerShieldEntity.Create(SharedState.Engine,
                playerShipEntity);
            playerShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerShieldEntity;
            playerShieldEntity.AddComponent(new PlayerComponent(PlayerArray[0]));

            if (PlayerArray.Length >= 2)
            {
                Entity playerTwoShipEntity = PlayerShipEntity.Create(SharedState.Engine,
                new Vector2(25, 25 * (PlayerArray.Length / 3)));
                Entity playerTwoShieldEntity = PlayerShieldEntity.Create(SharedState.Engine,
                    playerTwoShipEntity);
                playerTwoShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerTwoShieldEntity;
                playerTwoShieldEntity.AddComponent(new PlayerComponent(PlayerArray[1]));
            }

            if( PlayerArray.Length >= 3)
            {
                Entity playerThreeShipEntity = PlayerShipEntity.Create(SharedState.Engine, new Vector2(-25, -25));
                Entity playerThreeShieldEntity = PlayerShieldEntity.Create(SharedState.Engine, playerThreeShipEntity);
                playerThreeShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerThreeShieldEntity;
                playerThreeShieldEntity.AddComponent(new PlayerComponent(PlayerArray[2]));
            }

            if( PlayerArray.Length == 4)
            {
                Entity playerFourShipEntity = PlayerShipEntity.Create(SharedState.Engine, new Vector2(25, -25));
                Entity playerFourShieldEntity = PlayerShieldEntity.Create(SharedState.Engine, playerFourShipEntity);
                playerFourShipEntity.GetComponent<PlayerShipComponent>().ShipShield = playerFourShieldEntity;
                playerFourShieldEntity.AddComponent(new PlayerComponent(PlayerArray[3]));
            }

            EdgeEntity.Create(SharedState.Engine, new Vector2(0, CVars.Get<float>("screen_height") / 2), new Vector2(CVars.Get<float>("screen_width"), 5), new Vector2(0, -1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(-CVars.Get<float>("screen_width") / 2, 0), new Vector2(5, CVars.Get<float>("screen_height")), new Vector2(1, 0));
            EdgeEntity.Create(SharedState.Engine, new Vector2(0, -CVars.Get<float>("screen_height") / 2), new Vector2(CVars.Get<float>("screen_width"), 5), new Vector2(0, 1));
            EdgeEntity.Create(SharedState.Engine, new Vector2(CVars.Get<float>("screen_width") / 2, 0), new Vector2(5, CVars.Get<float>("screen_height")), new Vector2(-1, 0));
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
            if(evt is TogglePauseGameEvent)
            {
                HandlePause();
                return true;
            }

            return false;
        }

        private void CleanDestroyAllEntities()
        {
            // Explode all entities
            ImmutableList<Entity> explosionEntities = SharedState.Engine.GetEntitiesFor(Family
                .All(typeof(TransformComponent), typeof(ColoredExplosionComponent))
                .One(typeof(PlayerShipComponent),
                    typeof(PlayerShieldComponent),
                    typeof(EnemyComponent),
                    typeof(EdgeComponent))
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
            SharedState.Engine.DestroyEntitiesFor(Family.One(typeof(PlayerShipComponent),
                typeof(PlayerShieldComponent),
                typeof(EnemyComponent),
                typeof(EdgeComponent)).Get());
        }

        private void HandleGameOverEvent(GameOverEvent gameOverEvent)
        {
            ProcessManager.KillAll();

            CleanDestroyAllEntities();

            // TODO: Game Over Process
            Entity gameOverText = SharedState.Engine.CreateEntity();
            gameOverText.AddComponent(new TransformComponent(new Vector2(0, 1.25f * CVars.Get<float>("screen_height") / 2)));
            gameOverText.AddComponent(new FontComponent(Content.Load<BitmapFont>("font_game_over"), "Game Over"));
            ProcessManager.Attach(new GameOverAnimationProcess(gameOverText)).SetNext(new WaitProcess(3))
                .SetNext(new EntityDestructionProcess(SharedState.Engine, gameOverText))
                .SetNext(new DelegateCommand(() =>
            {
                ChangeState(new UILobbyGameState(GameManager, SharedState));
            }));
        }

        private void HandleIncreasePlayerScoreEvent(IncreasePlayerScoreEvent increasePlayerScoreEvent)
        {
            score += increasePlayerScoreEvent.ScoreAddend;
            ((Label)_root.FindWidgetByID("main_game_score_label")).Content = "Score: " + score;
        }

        private void HandlePause()
        {
            GameManager.ProcessManager.TogglePauseAll();
            GameManager.ProcessManager.Attach(new PauseGameState(GameManager, SharedState, this));
        }
    }
}
