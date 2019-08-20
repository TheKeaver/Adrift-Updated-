using System;
using GameJam.UI;
using GameJam.UI.Widgets;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace GameJam.States
{
    public class UIPlaygroundGameState : GameState
    {
        SpriteBatch _spriteBatch;

        BitmapFont _font;

        Root _root;

        Label _label;

        public UIPlaygroundGameState(GameManager gameManager) : base(gameManager)
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);
        }

        public override void Initialize()
        {
            _root = new Root(GameManager.GraphicsDevice.Viewport.Width,
                GameManager.GraphicsDevice.Viewport.Height);
        }

        public override void LoadContent()
        {
            _font = Content.Load<BitmapFont>(CVars.Get<string>("font_game_over"));
        }


        public override void Show()
        {
            _root.RegisterListeners();

            BuildUI();
        }

        public override void Hide()
        {
            _root.UnregisterListeners();
        }

        void BuildUI()
        {
            _label = new Label(_font,
                Origin.Center,
                0, 0,
                0, 0,
                1,
                0,
                AspectRatioType.HeightMaster);
            _label.Content = "Hello World!";
            _root.Add(_label);
        }

        public override void Update(float dt)
        {

        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _root.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public override void Dispose()
        {
            
        }
    }
}
