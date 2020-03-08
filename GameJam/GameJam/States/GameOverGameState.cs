using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Events.UI.GameOver;
using GameJam.Graphics.Text;
using GameJam.Processes.Animations;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class GameOverGameState : CommonGameState, IEventListener
    {
        public SpriteBatch SpriteBatch
        {
            get;
            private set;
        }
        private FieldFontRenderer _fieldFontRenderer;

        public Root Root
        {
            get;
            private set;
        }

        public Player[] Players
        {
            get;
            private set;
        }
        public int[] Score
        {
            get;
            private set;
        }

        public GameOverGameState(GameManager gameManager,
            SharedGameState sharedState,
            Player[] players,
            int[] score) : base(gameManager, sharedState)
        {
            Players = players;
            Score = score;
        }

        protected override void OnInitialize()
        {
            SpriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);
            Root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);

            LoadContent();

            Root.FindWidgetsByClass("main_fade_in").ForEach((Widget widget) =>
            {
                widget.Alpha = 0;
            });
            ProcessManager.Attach(new WidgetClassFadeAnimation(Root, "main_fade_in", 0, 1, CVars.Get<float>("game_over_ui_fade_in_duration"), Easings.Functions.SineEaseOut));

            UpdateScores();

            base.OnInitialize();
        }

        private void LoadContent()
        {
            Root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_game_over"));
        }

        protected override void OnKill()
        {
            base.OnKill();
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _fieldFontRenderer.Begin();
            SpriteBatch.Begin();
            Root.Render(SpriteBatch, _fieldFontRenderer);
            SpriteBatch.End();
            _fieldFontRenderer.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            Root.RegisterListeners();

            EventManager.Instance.RegisterListener<PlayAgainButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ExitToLobbyButtonPressedEvent>(this);

            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            Root.UnregisterListeners();

            base.UnregisterListeners();
        }

        private void UpdateScores()
        {
            for(int i = 0; i < Players.Length; i++)
            {
                Root.FindWidgetByID(string.Format("player_{0}_name", i)).Hidden = false;
                ((Label)Root.FindWidgetByID(string.Format("player_{0}_name", i))).Content = Players[i].Name;

                Root.FindWidgetByID(string.Format("player_{0}_score", i)).Hidden = false;
                ((Label)Root.FindWidgetByID(string.Format("player_{0}_score", i))).Content = Score[i].ToString();
            }
        }

        public bool Handle(IEvent evt)
        {
            if(evt is PlayAgainButtonPressedEvent)
            {
                PlayAgain();
            }
            if(evt is ExitToLobbyButtonPressedEvent)
            {
                ExitToLobby();
            }

            return false;
        }

        private void PlayAgain()
        {
            ChangeState(new AdriftGameState(GameManager, SharedState, Players));
        }

        private void ExitToLobby()
        {
            ChangeState(new UILobbyGameState(GameManager, SharedState, Players));
        }
    }
}
