using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics
{
    public class SMAA : PostProcessorEffect
    {
        readonly Effect _smaaEffect;
        RenderTarget2D _smaaTarget = null;
        Quad quad;

        public SMAA(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
            _smaaEffect = content.Load<Effect>("effect_smaa");
            quad = new Quad();
        }

        public override void Dispose() { }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if (_smaaTarget == null)
            {
                Resize(PostProcessor.Bounds.Width, PostProcessor.Bounds.Height);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(_smaaTarget);
            _smaaEffect.Parameters["colorTex2D"].SetValue(inTarget);
            foreach(EffectPass pass in _smaaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quad.Render(this.PostProcessor.GraphicsDevice);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(null);
            outTarget = _smaaTarget;
        }

        public override void Resize(int width, int height)
        {
            _smaaTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
                width,
                height,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);
        }

        public override void Update(float dt) { }
    }
}
