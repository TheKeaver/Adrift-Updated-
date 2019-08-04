using System;
using Microsoft.Xna.Framework.Graphics;
using GameJam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameJam.Input;

namespace GameJam.States
{
    public class MenuGameState : GameState
    {
        private SpriteBatch _spriteBatch;

        private Texture2D[] _menuTextures;
        private int _menuTextureIdx = 0;
        private Timer _menuChangeTimer;

        public MenuGameState(GameManager gameManager) : base(gameManager)
        {
        }

        public override void Dispose()
        {
            
        }

        public override void Hide()
        {
            
        }

        public override void Initialize()
        {
            _menuChangeTimer = new Timer(0.8f);
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);

            _menuTextures = new[]{
                Content.Load<Texture2D>(Constants.Resources.TEXTURE_TITLE_WITH_INSTRUCTIONS),
                Content.Load<Texture2D>(Constants.Resources.TEXTURE_TITLE_WITHOUT_INSTRUCTIONS)
            };
        }

        public override void Show()
        {
            
        }

        public override void Update(float dt)
        {
            _menuChangeTimer.Update(dt);
            if(_menuChangeTimer.HasElapsed())
            {
                _menuChangeTimer.Reset();

                _menuTextureIdx = (++_menuTextureIdx) % 2;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameManager.ChangeState(new MainGameState(GameManager, new Player("player", new PrimaryKeyboardInputMethod())));
            }
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_menuTextures[_menuTextureIdx],
                new Rectangle(0, 0, Constants.Global.WINDOW_WIDTH, Constants.Global.WINDOW_HEIGHT),
                Color.White);
            _spriteBatch.End();
        }
    }
}
