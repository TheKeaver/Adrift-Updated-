using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.UI;
using GameJam.Input;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    class UIMenuGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        int numberOfPlayers = 2;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UIMenuGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<PlayGameButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<OptionsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<QuitGameButtonPressedEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            RegisterEvents();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/MainMenu"));
            ((Button)_root.FindWidgetByID("PlayGame")).right = (Button)_root.FindWidgetByID("Options");
            ((Button)_root.FindWidgetByID("Options")).right = (Button)_root.FindWidgetByID("QuitGame");
            ((Button)_root.FindWidgetByID("QuitGame")).right = (Button)_root.FindWidgetByID("PlayGame");

            ((Button)_root.FindWidgetByID("PlayGame")).left = (Button)_root.FindWidgetByID("QuitGame");
            ((Button)_root.FindWidgetByID("Options")).left = (Button)_root.FindWidgetByID("PlayGame");
            ((Button)_root.FindWidgetByID("QuitGame")).left = (Button)_root.FindWidgetByID("Options");
        }

        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            UnregisterEvents();
        }

        public bool Handle(IEvent evt)
        {
            if(evt is PlayGameButtonPressedEvent)
            {
                Console.WriteLine("Play Game Pressed");
                Player[] players = new Player[numberOfPlayers];
                for (int i = 0; i < numberOfPlayers; i++)
                {
                    if (i == 0)
                        players[i] = new Player("playerOne", new PrimaryKeyboardInputMethod());
                    else
                        players[i] = new Player("playerTwo", new SecondaryKeyboardInputMethod());

                }
                // This line needs to instead change to Lobby Screen
                GameManager.ChangeState(new MainGameState(GameManager, players));
            }
            if(evt is OptionsButtonPressedEvent)
            {
                Console.WriteLine("Options Pressed");
            }
            if(evt is QuitGameButtonPressedEvent)
            {
                Console.WriteLine("Quit Game Pressed");
            }
            return false;
        }
    }
}
