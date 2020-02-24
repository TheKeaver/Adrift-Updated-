using Events;
using GameJam.Common;
using GameJam.Events;
using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;

namespace GameJam.Particles
{
    struct VertexIDVertexID : IVertexType
    {
        public float ID;
        public int VertexID;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float), VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1));

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexID(float id, int vertexID)
        {
            ID = id;
            VertexID = vertexID;
        }
    }
    struct VertexIDVertexIDPositionVelocityColor : IVertexType
    {
        public float ID;
        public int VertexID;
        public Vector2 Position;
        public Vector2 Velocity;
        public Color Color;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float), VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(sizeof(float) + sizeof(byte) * 4, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) + sizeof(byte) * 4 + sizeof(float) * 2, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
            new VertexElement(sizeof(float) + sizeof(byte) * 4 + sizeof(float) * 2 + sizeof(float) * 2, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            );
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexIDPositionVelocityColor(float id, int vertexID, Vector2 position, Vector2 velocity, Color color)
        {
            ID = id;
            VertexID = vertexID;
            Position = position;
            Velocity = velocity;
            Color = color;
        }
    }

    public class GPUParticleManager : IEventListener
    {
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        private Matrix _projectionMatrix;

        private Quad _screenQuad;
        private RenderTarget2D[] _positionVelocityTargets;
        private RenderTarget2D _staticInfoTarget;
        private int _currentPositionVelocityTarget;

        private Effect _particleEffect;

        private VertexBuffer _particleDrawVertices;
        private IndexBuffer _particleDrawIndices;
        private BlendState _particleCreateBlendState;
        private BlendState _particleUpdateBlendState;
        private BlendState _particleDrawBlendState;

        private List<VertexIDVertexIDPositionVelocityColor> _particleCreateVertices = new List<VertexIDVertexIDPositionVelocityColor>();
        private List<int> _particleCreateIndices = new List<int>();
        private int _lastParticleCreateID = 0;
        private int _particlesCreated = 0;
        private int _nextParticleCreateID = 0;

        public int ParticleBufferSize
        {
            get
            {
                return CVars.Get<int>("particle_gpu_buffer_size");
            }
        }
        public int ParticleCount
        {
            get
            {
                return ParticleBufferSize * ParticleBufferSize;
            }
        }


        public GPUParticleManager(GraphicsDevice graphicsDevice, ContentManager content, string particleEffect)
        {
            GraphicsDevice = graphicsDevice;

            _screenQuad = new Quad();
            _positionVelocityTargets = new RenderTarget2D[2];
            _positionVelocityTargets[0] = new RenderTarget2D(graphicsDevice,
                ParticleBufferSize,
                ParticleBufferSize,
                false,
                SurfaceFormat.Vector4,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_positionVelocityTargets[0]);
            _positionVelocityTargets[1] = new RenderTarget2D(graphicsDevice,
                ParticleBufferSize,
                ParticleBufferSize,
                false,
                SurfaceFormat.Vector4,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_positionVelocityTargets[1]);
            _staticInfoTarget = new RenderTarget2D(graphicsDevice,
                ParticleBufferSize,
                ParticleBufferSize,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_staticInfoTarget);

            _particleEffect = content.Load<Effect>(particleEffect);

            _particleDrawVertices = new VertexBuffer(GraphicsDevice,
                typeof(VertexIDVertexID),
                4 * ParticleCount,
                BufferUsage.WriteOnly);
            UploadParticleDrawVertices();
            _particleDrawIndices = new IndexBuffer(GraphicsDevice,
                typeof(int),
                ParticleCount * 6,
                BufferUsage.WriteOnly);
            UploadParticleDrawIndices();
            _particleUpdateBlendState = new BlendState
            {
                AlphaBlendFunction = BlendFunction.Max,
                AlphaSourceBlend = Blend.One,
                AlphaDestinationBlend = Blend.Zero,

                ColorBlendFunction = BlendFunction.Max,
                ColorSourceBlend = Blend.One,
                ColorDestinationBlend = Blend.Zero,
            };
            _particleCreateBlendState = _particleUpdateBlendState;
            _particleDrawBlendState = BlendState.NonPremultiplied;
        }

        public void RegisterListeners()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public void UnregisterListeners()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void CreateParticle(float x, float y, float velX, float velY, byte r, byte g, byte b)
        {
            int startI = _particleCreateVertices.Count;

            _particleCreateVertices.Add(new VertexIDVertexIDPositionVelocityColor()
            {
                ID = _nextParticleCreateID,
                VertexID = 0,
                Position = new Vector2(x, y),
                Velocity = new Vector2(velX, velY),
                Color = new Color(r, g, b)
            });
            _particleCreateVertices.Add(new VertexIDVertexIDPositionVelocityColor()
            {
                ID = _nextParticleCreateID,
                VertexID = 1,
                Position = new Vector2(x, y),
                Velocity = new Vector2(velX, velY),
                Color = new Color(r, g, b)
            });
            _particleCreateVertices.Add(new VertexIDVertexIDPositionVelocityColor()
            {
                ID = _nextParticleCreateID,
                VertexID = 2,
                Position = new Vector2(x, y),
                Velocity = new Vector2(velX, velY),
                Color = new Color(r, g, b)
            });
            _particleCreateVertices.Add(new VertexIDVertexIDPositionVelocityColor()
            {
                ID = _nextParticleCreateID,
                VertexID = 3,
                Position = new Vector2(x, y),
                Velocity = new Vector2(velX, velY),
                Color = new Color(r, g, b)
            });

            _particleCreateIndices.Add(startI);
            _particleCreateIndices.Add(startI + 1);
            _particleCreateIndices.Add(startI + 2);

            _particleCreateIndices.Add(startI);
            _particleCreateIndices.Add(startI + 2);
            _particleCreateIndices.Add(startI + 3);

            _nextParticleCreateID = (_nextParticleCreateID + 1) % ParticleCount;

            _particlesCreated++;
        }
        
        private void UploadParticleDrawVertices()
        {
            VertexIDVertexID[] verts = new VertexIDVertexID[4 * ParticleCount];
            int i = 0;
            for(int j = 0; j < ParticleCount; j++)
            {
                verts[i++] = new VertexIDVertexID(j, 0);
                verts[i++] = new VertexIDVertexID(j, 1);
                verts[i++] = new VertexIDVertexID(j, 2);
                verts[i++] = new VertexIDVertexID(j, 3);
            }
            _particleDrawVertices.SetData<VertexIDVertexID>(verts);
        }
        private void UploadParticleDrawIndices()
        {
            int[] idxs = new int[6 * ParticleCount];
            int i = 0;
            for (int j = 0; j < ParticleCount; j++)
            {
                idxs[i++] = j * 4;
                idxs[i++] = j * 4 + 1;
                idxs[i++] = j * 4 + 2;

                idxs[i++] = j * 4;
                idxs[i++] = j * 4 + 2;
                idxs[i++] = j * 4 + 3;
            }
            _particleDrawIndices.SetData<int>(idxs);
        }

        public void UpdateAndDraw(Camera camera, float dt, Camera debugCamera = null)
        {
            Matrix transformMatrix = debugCamera == null ? camera.TransformMatrix : debugCamera.TransformMatrix;

            BlendState prevBlendState = GraphicsDevice.BlendState;
            RenderTarget2D prevTarget = GraphicsDevice.RenderTargetCount > 0 ? (RenderTarget2D)GraphicsDevice.GetRenderTargets()[0].RenderTarget : null;

            _particleEffect.Parameters["Size"].SetValue(ParticleBufferSize);
            Create();
            Update(dt);
            _particleEffect.Parameters["WorldViewProjection"].SetValue(transformMatrix * _projectionMatrix);
            GraphicsDevice.SetRenderTarget(prevTarget);
            Draw();

            GraphicsDevice.BlendState = prevBlendState;
        }

        private void Create()
        {
            if(_particlesCreated > 0)
            {
                GraphicsDevice.BlendState = _particleCreateBlendState;

                // Position, etc.
                GraphicsDevice.SetRenderTarget(_positionVelocityTargets[_currentPositionVelocityTarget]);
                _particleEffect.CurrentTechnique = _particleEffect.Techniques["Create"];
                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                        _particleCreateVertices.ToArray(),
                        0,
                        _particleCreateVertices.Count,
                        _particleCreateIndices.ToArray(),
                        0,
                        _particleCreateIndices.Count / 3);
                }

                GraphicsDevice.SetRenderTarget(_staticInfoTarget);
                _particleEffect.CurrentTechnique = _particleEffect.Techniques["CreateStatic"];
                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                        _particleCreateVertices.ToArray(),
                        0,
                        _particleCreateVertices.Count,
                        _particleCreateIndices.ToArray(),
                        0,
                        _particleCreateIndices.Count / 3);
                }


                //// Done
                _lastParticleCreateID = _nextParticleCreateID;
                _particlesCreated = 0;

                _particleCreateVertices.Clear();
                _particleCreateIndices.Clear();
            }
        }
        private void Update(float dt)
        {
            _particleEffect.CurrentTechnique = _particleEffect.Techniques["Update"];

            int currentPositionSizeLifeTarget = _currentPositionVelocityTarget;
            int nextPositionSizeLifeTarget = (_currentPositionVelocityTarget + 1) % _positionVelocityTargets.Length;

            GraphicsDevice.SetRenderTarget(_positionVelocityTargets[nextPositionSizeLifeTarget]);
            GraphicsDevice.BlendState = _particleUpdateBlendState;
            _particleEffect.Parameters["Dt"].SetValue(dt);
            _particleEffect.Parameters["PositionVelocity"].SetValue(_positionVelocityTargets[currentPositionSizeLifeTarget]);
            _particleEffect.Parameters["StaticInfo"].SetValue(_staticInfoTarget);
            _particleEffect.Parameters["VelocityDecayMultiplier"].SetValue(CVars.Get<float>("particle_explosion_decay_multiplier"));
            foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _screenQuad.Render(GraphicsDevice);
            }
            _currentPositionVelocityTarget = nextPositionSizeLifeTarget;

            if (CVars.Get<bool>("particle_gpu_accelerated"))
			{
				GameManager.StatisticsProfiler.PushParticleCount(ParticleCount);
			}
        }

        private void Draw()
        {
            _particleEffect.CurrentTechnique = _particleEffect.Techniques["Draw"];

            GraphicsDevice.SetVertexBuffer(_particleDrawVertices);
            GraphicsDevice.Indices = _particleDrawIndices;
            GraphicsDevice.BlendState = _particleDrawBlendState;
            _particleEffect.Parameters["PositionVelocity"].SetValue(_positionVelocityTargets[_currentPositionVelocityTarget]);
            _particleEffect.Parameters["StaticInfo"].SetValue(_staticInfoTarget);
            _particleEffect.Parameters["ScaleX"].SetValue(CVars.Get<float>("particle_explosion_scale_x"));
            _particleEffect.Parameters["ScaleY"].SetValue(CVars.Get<float>("particle_explosion_scale_y"));
            foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                    0,
                    0,
                    ParticleCount * 2);
            }
        }

        private void ClearTarget(RenderTarget2D target)
        {
            ClearTarget(target, Color.Black);
        }
        private void ClearTarget(RenderTarget2D target, Color color)
        {
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(color);
            GraphicsDevice.SetRenderTarget(null);
        }

        public bool Handle(IEvent evt)
        {
            ResizeEvent resizeEvent = evt as ResizeEvent;
            if(resizeEvent != null)
            {
                HandleResizeEvent(resizeEvent);
            }

            return false;
        }

        private void HandleResizeEvent(ResizeEvent evt)
        {
            UpdateProjectionMatrix(evt.Width, evt.Height);
        }

        private void UpdateProjectionMatrix(int width, int height)
        {
            // Normal 3D cameras look into the -z direction (z = 1 is in front of z = 0). The
            // sprite batch layer depth is the opposite (z = 0 is in front of z = 1).
            // --> We get the correct matrix with near plane 0 and far plane -1.
            Matrix.CreateOrthographicOffCenter(0, width, 0,
                height, -1, 1, out _projectionMatrix);
        }
    }
}
