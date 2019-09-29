using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics
{
    public abstract class PostProcessorEffect : IDisposable
    {
        internal readonly PostProcessor PostProcessor;

        public bool Enabled
        {
            get;
            set;
        } = true;

        public PostProcessorEffect(PostProcessor postProcessor)
        {
            PostProcessor = postProcessor;
        }

        public abstract void Resize(int width, int height);

        public abstract void Update(float dt);

        public abstract void Process(RenderTarget2D inTarget, out RenderTarget2D outTarget);

        public abstract void Dispose();
    }
}
