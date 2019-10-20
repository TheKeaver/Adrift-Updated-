using System;
using System.Collections.Generic;
using Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;
using GameJam.Input;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using UI.Content.Pipeline;

namespace GameJam.States
{
    public class UIOptionsGameState : GameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        Root _root;

        private ProcessManager ProcessManager
        {
            get;
            set;
        }

        public UIOptionsGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
            EventManager.Instance.RegisterListener<KeyboardKeyDownEvent>(this);

            EventManager.Instance.RegisterListener<DisplaySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<ControlsSettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<DifficultySettingsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<SpeedSettingsButtonPressedEvent>(this);
        }

        void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override void Dispose()
        {
            UnregisterEvents();
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        public override void Initialize()
        {
            ProcessManager = new ProcessManager();

            RegisterEvents();

            _root = new Root(GameManager.GraphicsDevice.Viewport.Width, GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _.root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui/OptionsMenu"));
        }

        public override void Show()
        {
            _root.RegisterListeners();
        }

        public override void Update(float dt)
        {
            ProcessManager.Update(dt);
        }

        public bool Handle(IEvent evt)
        {
            // Listen for the 4 types of button settings pressed
            // Consider buttonSelectedEvent and buttonDeselectedEvent to allow showing of right side
            return false;
        }
    }
}
