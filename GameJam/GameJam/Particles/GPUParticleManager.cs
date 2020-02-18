using Events;
using GameJam.Common;
using GameJam.Events;
using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;

namespace GameJam.Particles
{
    struct VertexIDVertexID : IVertexType
    {
        public float ID;
        public int VertexID;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(byte) * 4, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1));

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexID(float id, int vertexID)
        {
            ID = id;
            VertexID = vertexID;
        }
    }
    struct VertexIDVertexIDPosition : IVertexType
    {
        public int ID;
        public int VertexID;
        public Vector2 Position;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(byte) * 4, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(sizeof(byte) * 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2)
            );
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexIDPosition(int id, int vertexID, Vector2 position)
        {
            ID = id;
            VertexID = vertexID;
            Position = position;
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
        private int _currentPositionVelocityTarget;

        private Effect _particleEffect;

        private VertexBuffer _particleDrawVertices;
        private IndexBuffer _particleDrawIndices;
        private BlendState _particleCreateBlendState;
        private BlendState _particleUpdateBlendState;
        private BlendState _particleDrawBlendState;

        private Texture2D _particleCreateTarget;
        private Vector4[] _particleCreateBuffer;
        private Texture2D _particleCreateMaskTarget;
        private byte[] _particleCreateMaskBuffer;
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

            _particleCreateTarget = new Texture2D(GraphicsDevice,
                ParticleBufferSize,
                ParticleBufferSize,
                false,
                SurfaceFormat.Vector4);
            _particleCreateBuffer = new Vector4[ParticleBufferSize * ParticleBufferSize];
            for (int i = 0; i < _particleCreateBuffer.Length; i++)
            {
                _particleCreateBuffer[i] = new Vector4();
            }
            _particleCreateMaskTarget = new Texture2D(GraphicsDevice,
                ParticleBufferSize,
                ParticleBufferSize,
                false,
                SurfaceFormat.Alpha8);
            _particleCreateMaskBuffer = new byte[ParticleBufferSize * ParticleBufferSize];
        }

        public void RegisterListeners()
        {
            EventManager.Instance.RegisterListener<ResizeEvent>(this);
        }
        public void UnregisterListeners()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void CreateParticle(float x, float y, float velX, float velY)
        {
            _particleCreateBuffer[_nextParticleCreateID] = new Vector4(x, y, velX, velY);
            _particleCreateMaskBuffer[_nextParticleCreateID] = 0xFF;
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

            _particleEffect.Parameters["Size"].SetValue(ParticleBufferSize);
            Create();
            Update(dt);
            _particleEffect.Parameters["WorldViewProjection"].SetValue(transformMatrix * _projectionMatrix);
            Draw();
        }

        private void Create()
        {
            if(_particlesCreated > 0)
            {
                _particleEffect.CurrentTechnique = _particleEffect.Techniques["Create"];

                // Create target: position, size, rotation data
                _particleCreateTarget.SetData(_particleCreateBuffer);
                // Create mask target: filters whether a particle is overwritten
                _particleCreateMaskTarget.SetData(_particleCreateMaskBuffer);

                // Position, etc.
                GraphicsDevice.SetRenderTarget(_positionVelocityTargets[_currentPositionVelocityTarget]);
                GraphicsDevice.BlendState = _particleCreateBlendState;
                _particleEffect.Parameters["PositionVelocity"].SetValue(_particleCreateTarget);
                _particleEffect.Parameters["CreateMask"].SetValue(_particleCreateMaskTarget);
                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    _screenQuad.Render(GraphicsDevice);
                }
                GraphicsDevice.SetRenderTarget(null);

                // Clear buffers to prepare for future particle creation
                for(int i = 0; i < _particlesCreated; i++)
                {
                    int id = (_lastParticleCreateID + i) % ParticleCount;

                    // Create buffer doesn't need to be cleared b/c the mask won't create it
                    _particleCreateMaskBuffer[id] = 0;
                }

                _lastParticleCreateID = _nextParticleCreateID;
                _particlesCreated = 0;
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
            _particleEffect.Parameters["VelocityDecayMultiplier"].SetValue(CVars.Get<float>("particle_explosion_decay_multiplier"));
            foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _screenQuad.Render(GraphicsDevice);
            }
            _currentPositionVelocityTarget = nextPositionSizeLifeTarget;
            GraphicsDevice.SetRenderTarget(null);
        }

        private void Draw()
        {
            _particleEffect.CurrentTechnique = _particleEffect.Techniques["Draw"];

            GraphicsDevice.SetVertexBuffer(_particleDrawVertices);
            GraphicsDevice.Indices = _particleDrawIndices;
            GraphicsDevice.BlendState = _particleDrawBlendState;
            _particleEffect.Parameters["PositionVelocity"].SetValue(_positionVelocityTargets[_currentPositionVelocityTarget]);
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
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Black);
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
