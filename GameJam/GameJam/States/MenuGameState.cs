using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameJam.Input;
using GameJam.Common;

namespace GameJam.States
{
    public class MenuGameState : CommonGameState
    {
        private SpriteBatch _spriteBatch;

        private Texture2D[] _menuTextures;
        private int _menuTextureIdx = 0;
        private int numberOfPlayers = 2;
        private Timer _menuChangeTimer;

        public MenuGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
        }

        protected override void OnInitialize()
        {
            _spriteBatch = new SpriteBatch(GameManager.GraphicsDevice);

            _menuTextures = new[]{
                Content.Load<Texture2D>(CVars.Get<string>("texture_title_with_instructions")),
                Content.Load<Texture2D>(CVars.Get<string>("texture_title_without_instructions"))
            };

            _menuChangeTimer = new Timer(0.8f);

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            _menuChangeTimer.Update(dt);
            if (_menuChangeTimer.HasElapsed())
            {
                _menuChangeTimer.Reset();

                _menuTextureIdx = (++_menuTextureIdx) % 2;
            }

            Player[] players = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (i == 0)
                    players[i] = new Player("playerOne", new PrimaryKeyboardInputMethod());
                else
                    players[i] = new Player("playerTwo", new SecondaryKeyboardInputMethod());

            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                ChangeState(new AdriftGameState(GameManager, SharedState, players));
            }

            base.OnUpdate(dt);
        }

        protected override void OnFixedUpdate(float dt)
        {
            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_menuTextures[_menuTextureIdx],
                new Rectangle(0, 0, CVars.Get<int>("window_width"), CVars.Get<int>("window_height")),
                Color.White);
            _spriteBatch.End();

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void OnKill()
        {
            base.OnKill();
        }
    }
}
