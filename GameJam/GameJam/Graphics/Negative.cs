using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GameJam.Graphics
{
    class Negative : PostProcessorEffect
    {
        readonly Effect _negativeEffect;
        RenderTarget2D negativeTarget = null;

        public float Radius = 1.0f;

        public Negative(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _negativeEffect = content.Load<Effect>("effect_negative");
        }

        public override void Dispose() { }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if (negativeTarget == null)
            {
                Resize(PostProcessor.Bounds.Width,
                    PostProcessor.Bounds.Height);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(negativeTarget);
            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _negativeEffect);
            PostProcessor.SpriteBatch.Draw(inTarget,
                negativeTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();
            PostProcessor.GraphicsDevice.SetRenderTarget(null);
            outTarget = negativeTarget;
        }

        public override void Resize(int width, int height)
        {
            negativeTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
        }

        public override void Update(float dt) { }
    }
}
