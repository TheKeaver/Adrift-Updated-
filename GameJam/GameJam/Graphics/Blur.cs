using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameJam.Graphics
{
    class Blur : PostProcessorEffect
    {
        readonly Effect _blurEffect;
        RenderTarget2D _hBlurTarget = null;
        RenderTarget2D _vBlurTarget = null;

        public float Radius = 1.0f;

        public Blur(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _blurEffect = content.Load<Effect>(CVars.Get<string>("effect_blur"));
        }

        public override void Dispose()
        {

        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if(_hBlurTarget == null || _vBlurTarget == null)
            {
                Resize(PostProcessor.Bounds.Width,
                    PostProcessor.Bounds.Height);
            }

            // Global uniforms
            _blurEffect.Parameters["radius"].SetValue(Radius);

            // Horizontal blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_hBlurTarget);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitX);
            _blurEffect.Parameters["resolution"].SetValue((float) _hBlurTarget.Width);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _blurEffect);
            PostProcessor.SpriteBatch.Draw(inTarget,
                _hBlurTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();

            // Vertical blur
            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitY);
            _blurEffect.Parameters["resolution"].SetValue((float)_vBlurTarget.Height);

            PostProcessor.GraphicsDevice.SetRenderTarget(_vBlurTarget);
            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _blurEffect);
            PostProcessor.SpriteBatch.Draw(_hBlurTarget,
                _vBlurTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();
            PostProcessor.GraphicsDevice.SetRenderTarget(null);

            outTarget = _vBlurTarget;
        }

        public override void Resize(int width, int height)
        {
            _hBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
            _vBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
        }

        public override void Update(float dt)
        {

        }
    }
}
