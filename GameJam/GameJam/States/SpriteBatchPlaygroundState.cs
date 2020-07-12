using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;

namespace GameJam.States
{
    public class SpriteBatchPlaygroundState : GameState
    {
        public SpriteBatch SpriteBatch {
            get;
            private set;
        }

        private Texture2D _shootingEnemyTexture;

        private Viewport _lastViewport;

        private Matrix _fieldFontRendererProjection;
        private BasicEffect _vectorSpriteEffect;

        public SpriteBatchPlaygroundState(GameManager gameManager) : base(gameManager)
        {
            _lastViewport = new Viewport();
        }

        protected override void OnInitialize()
        {
            SpriteBatch = new SpriteBatch(GameManager.GraphicsDevice);

            _shootingEnemyTexture = Content.Load<Texture2D>("texture_shooter_enemy");

            _vectorSpriteEffect = new BasicEffect(GameManager.GraphicsDevice);
            _vectorSpriteEffect.AmbientLightColor = new Vector3(1, 1, 1);
            _vectorSpriteEffect.World = Matrix.Identity;
            _vectorSpriteEffect.TextureEnabled = false;
            _vectorSpriteEffect.VertexColorEnabled = true;

            base.OnInitialize();
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            CheckUpdateProjections();

            /**
             * 
             * 0 = front,
             * 1 = back
             * 
             * Back to Front: front shows in front of back
             * Front to back: back shows in front of front
             * 
             **/
            SpriteBatch.Begin(SpriteSortMode.BackToFront,
                null,
                null,
                null,
                null,
                null,
                null);

            SpriteBatch.Draw(_shootingEnemyTexture,
                new Rectangle(10, 10, 100, 100),
                null,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0);
            SpriteBatch.Draw(_shootingEnemyTexture,
                new Rectangle(40, 10, 100, 100),
                null,
                Color.Yellow,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0.5f);
            SpriteBatch.Draw(_shootingEnemyTexture,
                new Rectangle(70, 10, 100, 100),
                null,
                Color.Cyan,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0);

            SpriteBatch.End();

            /******** QUAD DRAW *********/
            VertexPositionColor[] verts = new VertexPositionColor[]
            {
                new VertexPositionColor(new Vector3(30, 80, 0.5f), Color.LightGreen),
                new VertexPositionColor(new Vector3(30, 30, 0.5f), Color.LightGreen),
                new VertexPositionColor(new Vector3(80, 30, 0.5f), Color.LightGreen),

                new VertexPositionColor(new Vector3(80, 30, 0.5f), Color.LightGreen),
                new VertexPositionColor(new Vector3(80, 80, 0.5f), Color.LightGreen),
                new VertexPositionColor(new Vector3(30, 80, 0.5f), Color.LightGreen)
            };
            DepthStencilState depthStencilState = new DepthStencilState
            {
                DepthBufferEnable = true,
                DepthBufferWriteEnable = true
            };
            GameManager.GraphicsDevice.DepthStencilState = depthStencilState;
            foreach(EffectPass pass in _vectorSpriteEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameManager.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList,
                    verts,
                    0,
                    verts.Length / 3);
            }

            base.OnRender(dt, betweenFrameAlpha);
        }

        #region PROJECTION_UPDATES
        private void CheckUpdateProjections()
        {
            Viewport viewport = GameManager.GraphicsDevice.Viewport;
            if (viewport.Width != _lastViewport.Width || viewport.Height != _lastViewport.Height)
            {
                SetupFieldFontDrawing(viewport);
                SetupVectorAndSpriteDrawing(viewport);

                _lastViewport = viewport;
            }
        }
        private void SetupFieldFontDrawing(Viewport viewport)
        {
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, 0,
                viewport.Height, -float.MaxValue, float.MaxValue, out _fieldFontRendererProjection);
        }
        private void SetupVectorAndSpriteDrawing(Viewport viewport)
        {
            Matrix projection;
            Matrix.CreateOrthographicOffCenter(0, viewport.Width, viewport.Height,
                0, -float.MaxValue, float.MaxValue, out projection);
            _vectorSpriteEffect.Projection = projection;
            //_spriteEffect.Projection = projection;

            _lastViewport = viewport;
        }
        #endregion
    }
}
