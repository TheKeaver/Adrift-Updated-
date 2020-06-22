using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameJam.Content;

namespace GameJam.Graphics
{
    public class SMAA : PostProcessorEffect
    {
        readonly Effect _smaaEffect;
        RenderTarget2D _edgesTex = null;
        RenderTarget2D _blendTex = null;
        RenderTarget2D _smaaTarget = null;
        Quad quad;

        Texture2D _areaTex;
        Texture2D _searchTex;
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

                _areaTex = AreaTextureSMAA.GetAreaTexture(PostProcessor.GraphicsDevice);
                _searchTex = SearchTextureSMAA.GetSearchTexture(PostProcessor.GraphicsDevice);
            }

            PostProcessor.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

            // Input > SMAA_Color_EdgeDecection > EdgesTex
            _smaaEffect.Parameters["colorTex2D"].SetValue(inTarget);
            PostProcessor.GraphicsDevice.SetRenderTarget(_edgesTex);
            _smaaEffect.CurrentTechnique = _smaaEffect.Techniques["ColorEdgeDetection"];
            foreach(EffectPass pass in _smaaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quad.Render(this.PostProcessor.GraphicsDevice);
            }

            // EdgesTex > SMAABlendingWeightCalculation > BlendTex
            _smaaEffect.Parameters["edgesTex2D"].SetValue(_edgesTex);
            _smaaEffect.Parameters["areaTex2D"].SetValue(_areaTex);
            _smaaEffect.Parameters["searchTex2D"].SetValue(_searchTex);
            PostProcessor.GraphicsDevice.SetRenderTarget(_blendTex);
            _smaaEffect.CurrentTechnique = _smaaEffect.Techniques["BlendWeightCalculation"];
            foreach(EffectPass pass in _smaaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quad.Render(this.PostProcessor.GraphicsDevice);
            }

            // BlendTex > SMAANeighborhoodBlending > Output
            _smaaEffect.Parameters["blendTex2D"].SetValue(_blendTex);
            PostProcessor.GraphicsDevice.SetRenderTarget(_smaaTarget);
            _smaaEffect.CurrentTechnique = _smaaEffect.Techniques["NeighborhoodBlending"];
            foreach(EffectPass pass in _smaaEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                quad.Render(this.PostProcessor.GraphicsDevice);
            }

            PostProcessor.GraphicsDevice.SetRenderTarget(_edgesTex);
            PostProcessor.GraphicsDevice.Clear(Color.TransparentBlack);
            PostProcessor.GraphicsDevice.SetRenderTarget(_blendTex);
            PostProcessor.GraphicsDevice.Clear(Color.TransparentBlack);

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
