using System;
using Microsoft.Xna.Framework.Graphics;
using GameJam;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameJam.Input;
using GameJam.Common;

namespace GameJam.States
{
    public class MenuGameState : GameState
    {
        private SpriteBatch _spriteBatch;

        private Texture2D[] _menuTextures;
        private int _menuTextureIdx = 0;
        private int numberOfPlayers = 2;
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
                Content.Load<Texture2D>("texture_title_with_instructions"),
                Content.Load<Texture2D>("texture_title_without_instructions")
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

            Player[] players = new Player[numberOfPlayers];
            for ( int i=0; i<numberOfPlayers; i++)
            {
                if (i == 0)
                    players[i] = new Player("playerOne", new PrimaryKeyboardInputMethod());
                else
                    players[i] = new Player("playerTwo", new SecondaryKeyboardInputMethod());

            }

            if(Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                GameManager.ChangeState( new MainGameState(GameManager, players) );
            }
        }

        public override void Draw(float dt)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_menuTextures[_menuTextureIdx],
                new Rectangle(0, 0, CVars.Get<int>("window_width"), CVars.Get<int>("window_height")),
                Color.White);
            _spriteBatch.End();
        }
    }
}
