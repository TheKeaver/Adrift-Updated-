using System;
using System.Collections.Generic;
using Events;
using GameJam.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Graphics
{
    public class PostProcessor : IEventListener, IDisposable
    {
        public readonly GraphicsDevice GraphicsDevice;
        internal SpriteBatch SpriteBatch;

        public bool Drawing
        {
            get;
            private set;
        }

        public Rectangle Bounds
        {
            get
            {
                return _renderTarget.Bounds;
            }
            private set
            {
                _renderTarget = new RenderTarget2D(GraphicsDevice,
                                             value.Width,
                                             value.Height,
                                             false,
                                             SurfaceFormat.Color,
                                             DepthFormat.None);

                for (int i = 0; i < Effects.Count; i++)
                {
                    Effects[i].Resize(value.Width, value.Height);
                }
            }
        }

        RenderTarget2D _renderTarget;

        public List<PostProcessorEffect> Effects = new List<PostProcessorEffect>();

        public PostProcessor(GraphicsDevice graphicsDevice, float bufferWidth, float bufferHeight)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Bounds = new Rectangle(0, 0,
                                (int)bufferWidth,
                                (int)bufferHeight);
        }

        public void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void Begin()
        {
            if (Drawing)
            {
                throw new Exception("Already Running");
            }
            Drawing = true;

            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.Clear(Color.Transparent);
        }

        public void Update(float dt)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].Enabled)
                {
                    Effects[i].Update(dt);
                }
            }
        }

        public RenderTarget2D End()
        {
            return End(true);
        }

        public RenderTarget2D End(bool draw)
        {
            if (!Drawing)
            {
                throw new Exception("Not Drawing");
            }
            Drawing = false;

            GraphicsDevice.SetRenderTarget(null);

            RenderTarget2D finalTarget = _renderTarget;
            // Post-process
            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].Enabled)
                {
                    Effects[i].Process(finalTarget, out finalTarget);
                }
            }

            // Render as a fullscreen quad
            if (draw)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(finalTarget, Bounds, Color.White);
                SpriteBatch.End();
            }
            return finalTarget;
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;

            if (resizeEvent != null)
            {
                Bounds = new Rectangle(0, 0,
                                       resizeEvent.Width,
                                       resizeEvent.Height);
            }

            return false;
        }

        public void Dispose()
        {
            SpriteBatch.Dispose();
            _renderTarget.Dispose();

            for (int i = 0; i < Effects.Count; i++)
            {
                Effects[i].Dispose();
            }
        }
    }
}
