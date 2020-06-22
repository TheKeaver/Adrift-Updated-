using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Graphics
{
    class Bloom : PostProcessorEffect
    {
        readonly Effect _blurEffect;

        RenderTarget2D _inTarget = null;

        RenderTarget2D _hFirstBlurTarget = null;
        RenderTarget2D _vFirstBlurTarget = null;
        RenderTarget2D FirstBlurStorage = null;

        RenderTarget2D _hSecondBlurTarget = null;
        RenderTarget2D _vSecondBlurTarget = null;
        RenderTarget2D SecondBlurStorage = null;

        RenderTarget2D FinalRender = null;

        public float Radius = 1.0f;

        public Bloom(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _blurEffect = content.Load<Effect>("effect_blur");
        }

        public override void Dispose()
        {

        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            _inTarget = inTarget; // STORES ORIGINAL

            if (_hFirstBlurTarget == null || _vFirstBlurTarget == null || _hSecondBlurTarget == null || _vSecondBlurTarget == null || FinalRender == null)
            {
                Resize(PostProcessor.Bounds.Width,
                    PostProcessor.Bounds.Height);
            }

            _blurEffect.Parameters["radius"].SetValue(Radius);

            // Horizontal 1st Blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_hFirstBlurTarget);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitX);
            _blurEffect.Parameters["resolution"].SetValue((float)_hFirstBlurTarget.Width);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            _blurEffect);
            PostProcessor.SpriteBatch.Draw(inTarget, _hFirstBlurTarget.Bounds, Color.White);
            PostProcessor.SpriteBatch.End();

            // Vertical 1st Blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_vFirstBlurTarget);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitY);
            _blurEffect.Parameters["resolution"].SetValue((float)_vFirstBlurTarget.Height);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            _blurEffect);
            PostProcessor.SpriteBatch.Draw(_hFirstBlurTarget, _vFirstBlurTarget.Bounds, Color.White);
            PostProcessor.SpriteBatch.End();

            FirstBlurStorage = _vFirstBlurTarget; // STORES FIRST BLUR

            // Horizontal 2nd Blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_hSecondBlurTarget);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitX);
            _blurEffect.Parameters["resolution"].SetValue((float)_hSecondBlurTarget.Width);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            _blurEffect);
            PostProcessor.SpriteBatch.Draw(FirstBlurStorage, _hSecondBlurTarget.Bounds, Color.White);
            PostProcessor.SpriteBatch.End();

            // Vertical 2nd Blur
            PostProcessor.GraphicsDevice.SetRenderTarget(_vSecondBlurTarget);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            _blurEffect.Parameters["direction"].SetValue(Vector2.UnitX);
            _blurEffect.Parameters["resolution"].SetValue((float)_vSecondBlurTarget.Width);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                                            null,
                                            null,
                                            null,
                                            null,
                                            _blurEffect);
            PostProcessor.SpriteBatch.Draw(_hSecondBlurTarget, _vSecondBlurTarget.Bounds, Color.White);
            PostProcessor.SpriteBatch.End();

            SecondBlurStorage = _vSecondBlurTarget; // STORES SECOND BLUR

            // Draw Final Product into FinalRender
            PostProcessor.GraphicsDevice.SetRenderTarget(FinalRender);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            PostProcessor.SpriteBatch.Draw(SecondBlurStorage, FinalRender.Bounds, Color.White);
            PostProcessor.SpriteBatch.Draw(FirstBlurStorage, FinalRender.Bounds, Color.White);
            PostProcessor.SpriteBatch.Draw(_inTarget, FinalRender.Bounds, Color.White);
            PostProcessor.SpriteBatch.End();

            PostProcessor.GraphicsDevice.SetRenderTarget(null);

            outTarget = FinalRender;
        }

        public override void Resize(int width, int height)
        {
            _hFirstBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice, width/2, height/2, false, SurfaceFormat.Color, DepthFormat.None);
            _vFirstBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice, width/2, height/2, false, SurfaceFormat.Color, DepthFormat.None);
            _hSecondBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice, width/2, height/2, false, SurfaceFormat.Color, DepthFormat.None);
            _vSecondBlurTarget = new RenderTarget2D(PostProcessor.GraphicsDevice, width/2, height/2, false, SurfaceFormat.Color, DepthFormat.None);
            FinalRender = new RenderTarget2D(PostProcessor.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None);
        }

        public override void Update(float dt)
        {
            throw new NotImplementedException();
        }
    }
}
