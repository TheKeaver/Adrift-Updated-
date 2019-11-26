using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Graphics
{
    class FXAA : PostProcessorEffect
    {
        readonly Effect _fxaaEffect;
        RenderTarget2D _fxaaTarget = null;

        // Choose the amount of sub-pixel aliasing removal.
        // This can effect sharpness.
        //   1.00 - upper limit (softer)
        //   0.75 - default amount of filtering
        //   0.50 - lower limit (sharper, less sub-pixel aliasing removal)
        //   0.25 - almost off
        //   0.00 - completely off
        private float subPixelAliasingRemoval = 0.25f;

        // The minimum amount of local contrast required to apply algorithm.
        //   0.333 - too little (faster)
        //   0.250 - low quality
        //   0.166 - default
        //   0.125 - high quality 
        //   0.063 - overkill (slower)
        private float edgeTheshold = 0.166f;

        // Trims the algorithm from processing darks.
        //   0.0833 - upper limit (default, the start of visible unfiltered edges)
        //   0.0625 - high quality (faster)
        //   0.0312 - visible limit (slower)
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        private float edgeThesholdMin = 0f;

#if XBOX
        // This effects sub-pixel AA quality and inversely sharpness.
        //   Where N ranges between,
        //     N = 0.50 (default)
        //     N = 0.33 (sharper)
        private float N = 0.40f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_SHARPNESS for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only three safe values here: 2 and 4 and 8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // For all other platforms can be a non-power of two.
        //   8.0 is sharper (default!!!)
        //   4.0 is softer
        //   2.0 is really soft (good only for vector graphics inputs)
        private float consoleEdgeSharpness = 8.0f;

        // This does not effect PS3, as this needs to be compiled in.
        //   Use FXAA_CONSOLE__PS3_EDGE_THRESHOLD for PS3.
        //   Due to the PS3 being ALU bound,
        //   there are only two safe values here: 1/4 and 1/8.
        //   These options use the shaders ability to a free *|/ by 2|4|8.
        // The console setting has a different mapping than the quality setting.
        // Other platforms can use other values.
        //   0.125 leaves less aliasing, but is softer (default!!!)
        //   0.25 leaves more aliasing, and is sharper
        private float consoleEdgeThreshold = 0.125f;

        // Trims the algorithm from processing darks.
        // The console setting has a different mapping than the quality setting.
        // This only applies when FXAA_EARLY_EXIT is 1.
        // This does not apply to PS3, 
        // PS3 was simplified to avoid more shader instructions.
        //   0.06 - faster but more aliasing in darks
        //   0.05 - default
        //   0.04 - slower and less aliasing in darks
        // Special notes when using FXAA_GREEN_AS_LUMA,
        //   Likely want to set this to zero.
        //   As colors that are mostly not-green
        //   will appear very dark in the green channel!
        //   Tune by looking at mostly non-green content,
        //   then start at zero and increase until aliasing is a problem.
        private float consoleEdgeThresholdMin = 0f;
#endif

        public FXAA(PostProcessor postProcessor, ContentManager content) : base(postProcessor)
        {
#if WINDOWS_UWP
            throw new Exception("FXAA not supported on UWP.");
#else
            _fxaaEffect = content.Load<Effect>("effect_fxaa");
#endif
        }

        public override void Dispose()
        {

        }

        public override void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget)
        {
            if (_fxaaTarget == null)
            {
                Resize(PostProcessor.Bounds.Width,
                    PostProcessor.Bounds.Height);
            }
            
            PostProcessor.GraphicsDevice.SetRenderTarget(_fxaaTarget);
            PostProcessor.GraphicsDevice.Clear(Color.Transparent);

            _fxaaEffect.CurrentTechnique = _fxaaEffect.Techniques["FXAA"];

            _fxaaEffect.Parameters["InverseViewportSize"].SetValue(new Vector2(1f / _fxaaTarget.Width, 1f / _fxaaTarget.Height));
            _fxaaEffect.Parameters["SubPixelAliasingRemoval"].SetValue(subPixelAliasingRemoval);
            _fxaaEffect.Parameters["EdgeThreshold"].SetValue(edgeTheshold);
            _fxaaEffect.Parameters["EdgeThresholdMin"].SetValue(edgeThesholdMin);
#if XBOX
            /** NOT SUPPORTED **/
            throw new Exception();
            _fxaaEffect.Parameters["ConsoleSharpness"].SetValue(new Vector4(
                -N / _fxaaTarget.Width,
                -N / _fxaaTarget.Height,
                N / _fxaaTarget.Width,
                N / _fxaaTarget.Height
                ));
            _fxaaEffect.Parameters["ConsoleOpt1"].SetValue(new Vector4(
                -2.0f / _fxaaTarget.Width,
                -2.0f / _fxaaTarget.Height,
                2.0f / _fxaaTarget.Width,
                2.0f / _fxaaTarget.Height
                ));
            _fxaaEffect.Parameters["ConsoleOpt2"].SetValue(new Vector4(
                8.0f / _fxaaTarget.Width,
                8.0f / _fxaaTarget.Height,
                -4.0f / _fxaaTarget.Width,
                -4.0f / _fxaaTarget.Height
                ));
            _fxaaEffect.Parameters["ConsoleEdgeSharpness"].SetValue(consoleEdgeSharpness);
            _fxaaEffect.Parameters["ConsoleEdgeThreshold"].SetValue(consoleEdgeThreshold);
            _fxaaEffect.Parameters["ConsoleEdgeThresholdMin"].SetValue(consoleEdgeThresholdMin);
#endif

            PostProcessor.SpriteBatch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                _fxaaEffect);
            PostProcessor.SpriteBatch.Draw(inTarget,
                _fxaaTarget.Bounds,
                Color.White);
            PostProcessor.SpriteBatch.End();

            PostProcessor.GraphicsDevice.SetRenderTarget(null);

            outTarget = _fxaaTarget;
        }

        public override void Resize(int width, int height)
        {
            _fxaaTarget = new RenderTarget2D(PostProcessor.GraphicsDevice,
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
