using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics
{
    public class MSAA : PostProcessorEffect
    {
        private RenderTarget2D _aaTarget = null;

        private RasterizerState _rasterizerState;

        public MSAA(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _rasterizerState = new RasterizerState
            {
                MultiSampleAntiAlias = true
            };
        }

        public override void Dispose()
        {
        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if(_aaTarget == null)
            {
                Resize(PostProcessor.Bounds.Width, PostProcessor.Bounds.Height);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(_aaTarget);
            PostProcessor.GraphicsDevice.Clear(Color.TransparentBlack);
            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                _rasterizerState);
            PostProcessor.SpriteBatch.Draw(inTarget,
                _aaTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();

            PostProcessor.GraphicsDevice.SetRenderTarget(null);
            outTarget = _aaTarget;
        }

        public override void Resize(int width, int height)
        {
            _aaTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
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
