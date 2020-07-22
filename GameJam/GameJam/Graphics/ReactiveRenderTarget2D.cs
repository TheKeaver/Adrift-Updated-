using Events;
using GameJam.Events;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics
{
    public class ReactiveRenderTarget2D : IEventListener
    {
        public RenderTarget2D RenderTarget
        {
            get;
            private set;
        }

        public readonly GraphicsDevice GraphicsDevice;

        private bool _mipMap;
        public bool MipMap
        {
            get
            {
                return _mipMap;
            }
            set
            {
                _mipMap = value;
                GenerateRenderTarget();
            }
        }

        private SurfaceFormat _surfaceFormat;
        public SurfaceFormat SurfaceFormat
        {
            get
            {
                return _surfaceFormat;
            }
            set
            {
                _surfaceFormat = value;
                GenerateRenderTarget();
            }
        }

        private DepthFormat _depthFormat;
        public DepthFormat DepthFormat
        {
            get
            {
                return _depthFormat;
            }
            set
            {
                _depthFormat = value;
                GenerateRenderTarget();
            }
        }

        public ReactiveRenderTarget2D(GraphicsDevice graphicsDevice,
            bool mipMap,
            SurfaceFormat surfaceFormat,
            DepthFormat depthFormat)
        {
            GraphicsDevice = graphicsDevice;
            _mipMap = mipMap;
            _surfaceFormat = surfaceFormat;
            _depthFormat = depthFormat;

            GenerateRenderTarget();
        }

        private void GenerateRenderTarget()
        {
            RenderTarget = new RenderTarget2D(GraphicsDevice,
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height,
                MipMap,
                SurfaceFormat,
                _depthFormat);
        }

        public void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public bool Handle(IEvent evt)
        {
            if(evt is ResizeEvent)
            {
                GenerateRenderTarget();
            }

            return false;
        }
    }
}
