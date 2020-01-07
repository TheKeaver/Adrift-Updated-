using System;
using FontExtension;
using GameJam.Graphics.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.States
{
    public class MSDFFontTestGameState : GameState
    {
        private TextRenderer _textRenderer;
        private float _elapsed;

        public MSDFFontTestGameState(GameManager gameManager) : base(gameManager)
        {
        }

        protected override void OnInitialize()
        {
            Effect fieldFontEffect = Content.Load<Effect>("effect_field_font");
            FieldFont font = Content.Load<FieldFont>("font_msdf_hyperspace");

            _textRenderer = new TextRenderer(fieldFontEffect, font, GameManager.GraphicsDevice);

            base.OnInitialize();
        }

        protected override void OnFixedUpdate(float dt)
        {
            _elapsed += dt;

            base.OnFixedUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            GameManager.GraphicsDevice.Clear(Color.Black);

            GameManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GameManager.GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GameManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            var viewport = GameManager.GraphicsDevice.Viewport;

            //var world = Matrix.CreateScale(0.01f) *  Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds);
            var world = Matrix.CreateScale(0.01f) * Matrix.Identity;
            var view = Matrix.CreateLookAt(Vector3.Backward, Vector3.Forward, Vector3.Up);
            var projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                viewport.Width / (float)viewport.Height,
                0.01f,
                1000.0f);

            var wvp = world * view * projection;
            _textRenderer.OptimizeForTinyText = true;

            _textRenderer.Render("~:;!435&^%$", wvp);


            world = Matrix.CreateScale(0.01f) * Matrix.CreateRotationY(_elapsed / 10) * Matrix.CreateRotationZ(MathHelper.PiOver4);
            view = Matrix.CreateLookAt(Vector3.Backward, Vector3.Forward, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver2,
                viewport.Width / (float)viewport.Height,
                0.01f,
                1000.0f);

            wvp = world * view * projection;

            _textRenderer.Render("abcdefghijklmnopqrstuvwxyz", wvp);

            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            base.UnregisterListeners();
        }
    }
}
