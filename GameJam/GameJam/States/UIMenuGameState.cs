using System.Collections.Generic;
using Adrift.Content.Common.UI;
using Events;
using GameJam.Events;
using GameJam.Events.InputHandling;
using GameJam.Events.UI;
using GameJam.Graphics.Text;
using GameJam.Processes.Menu;
using GameJam.UI;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    class UIMenuGameState : CommonGameState, IEventListener
    {
        SpriteBatch _spriteBatch;
        FieldFontRenderer _fieldFontRenderer;
        Root _root;

        public UIMenuGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
            _fieldFontRenderer = new FieldFontRenderer(Content, GameManager.GraphicsDevice);
        }

        protected override void OnInitialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
            _root.BuildFromPrototypes(Content, Content.Load<List<WidgetPrototype>>("ui_main_menu"));

            _root.AutoControlModeSwitching = true;

            ProcessManager.Attach(new EntityBackgroundSpawner(SharedState.Engine));

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            GameManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            _fieldFontRenderer.Begin();
            _spriteBatch.Begin(SpriteSortMode.BackToFront);
            _root.Render(_spriteBatch, _fieldFontRenderer);
            _spriteBatch.End();
            _fieldFontRenderer.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            _root.RegisterListeners();

            EventManager.Instance.RegisterListener<PlayGameButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<OptionsButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<QuitGameButtonPressedEvent>(this);
            EventManager.Instance.RegisterListener<GamePadButtonDownEvent>(this);
        }

        protected override void UnregisterListeners()
        {
            _root.UnregisterListeners();

            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if(evt is PlayGameButtonPressedEvent)
            {
                ChangeState(new UILobbyGameState(GameManager, SharedState));
            }
            if(evt is OptionsButtonPressedEvent)
            {
                ChangeState(new UIOptionsGameState(GameManager, SharedState));
            }
            if(evt is QuitGameButtonPressedEvent)
            {
                EventManager.Instance.QueueEvent(new GameShutdownEvent());
            }
            return false;
        }
    }
}
