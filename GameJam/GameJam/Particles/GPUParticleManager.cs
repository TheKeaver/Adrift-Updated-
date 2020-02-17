using GameJam.Common;
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

    public class GPUParticleManager
    {
        public GraphicsDevice GraphicsDevice
        {
            get;
            private set;
        }

        private Quad _screenQuad;
        private RenderTarget2D[] _positionSizeLifeTargets;
        private int _currentPositionSizeLifeTarget;

        private Effect _particleEffect;

        private VertexBuffer _particleDrawVertices;
        private IndexBuffer _particleDrawIndices;

        private Texture2D _particleCreateTarget;
        private HalfVector4[] _particleCreateBuffer;
        private bool _recreate = true;

        private float _elapsedTime;

        private int _particleCount = 100000;
        private int _particleBufferSize = 1024;

        private int _nextParticleID = 0;

        public GPUParticleManager(GraphicsDevice graphicsDevice, ContentManager content, string particleEffect)
        {
            GraphicsDevice = graphicsDevice;

            _screenQuad = new Quad();
            _positionSizeLifeTargets = new RenderTarget2D[2];
            _positionSizeLifeTargets[0] = new RenderTarget2D(graphicsDevice,
                _particleBufferSize,
                _particleBufferSize,
                false,
                SurfaceFormat.HalfVector4,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_positionSizeLifeTargets[0]);
            _positionSizeLifeTargets[1] = new RenderTarget2D(graphicsDevice,
                _particleBufferSize,
                _particleBufferSize,
                false,
                SurfaceFormat.HalfVector4,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_positionSizeLifeTargets[1]);

            _particleEffect = content.Load<Effect>(particleEffect);

            _particleDrawVertices = new VertexBuffer(GraphicsDevice,
                typeof(VertexIDVertexID),
                4 * _particleCount,
                BufferUsage.WriteOnly);
            UploadParticleDrawVertices();
            _particleDrawIndices = new IndexBuffer(GraphicsDevice,
                typeof(int),
                _particleCount * 6,
                BufferUsage.WriteOnly);
            UploadParticleDrawIndices();

            _particleCreateTarget = new Texture2D(GraphicsDevice,
                _particleBufferSize,
                _particleBufferSize,
                false,
                SurfaceFormat.HalfVector4);
            _particleCreateBuffer = new HalfVector4[_particleBufferSize * _particleBufferSize];
            for(int i = 0; i < _particleCreateBuffer.Length; i++)
            {
                _particleCreateBuffer[i] = new HalfVector4();
            }

            {
                //Random random = new Random();
                //for(int i = 0; i < _particleCount; i++)
                //{
                //    CreateParticle(new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1)));
                //}
                Vector2 currentPos = new Vector2(-1, -1);
                for (int i = 0; i < _particleCount; i++)
                {
                    currentPos.X += 0.01f;
                    if(currentPos.X > 1)
                    {
                        currentPos.X = -1;
                        currentPos.Y += 0.01f;
                    }
                    CreateParticle(new Vector2(currentPos.X, currentPos.Y));
                }
            }
        }

        public void CreateParticle(Vector2 position)
        {
            _particleCreateBuffer[_nextParticleID] = new HalfVector4(position.X, position.Y, 0.1f, 0);
            _nextParticleID = (_nextParticleID + 1) % _particleCount;
        }
        
        private void UploadParticleDrawVertices()
        {
            VertexIDVertexID[] verts = new VertexIDVertexID[4 * _particleCount];
            int i = 0;
            for(int j = 0; j < _particleCount; j++)
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
            int[] idxs = new int[6 * _particleCount];
            int i = 0;
            for (int j = 0; j < _particleCount; j++)
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

        public void UpdateAndDraw(float dt)
        {
            _elapsedTime += dt;

            _particleEffect.Parameters["Size"].SetValue(_particleBufferSize);
            Create();
            Update(dt);
            Draw();
        }

        private void Create()
        {
            if(_recreate)
            {
                _particleEffect.CurrentTechnique = _particleEffect.Techniques["Create"];

                _particleCreateTarget.SetData(_particleCreateBuffer);

                GraphicsDevice.SetRenderTarget(_positionSizeLifeTargets[_currentPositionSizeLifeTarget]);
                _particleEffect.Parameters["PositionSizeRotation"].SetValue(_particleCreateTarget);
                foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    _screenQuad.Render(GraphicsDevice);
                }
                GraphicsDevice.SetRenderTarget(null);

                _recreate = false;
            }
        }
        private void Update(float dt)
        {
            _particleEffect.CurrentTechnique = _particleEffect.Techniques["Update"];

            int currentPositionSizeLifeTarget = _currentPositionSizeLifeTarget;
            int nextPositionSizeLifeTarget = (_currentPositionSizeLifeTarget + 1) % _positionSizeLifeTargets.Length;

            GraphicsDevice.SetRenderTarget(_positionSizeLifeTargets[nextPositionSizeLifeTarget]);
            _particleEffect.Parameters["ElapsedTime"].SetValue(_elapsedTime);
            _particleEffect.Parameters["Dt"].SetValue(dt);
            _particleEffect.Parameters["PositionSizeRotation"].SetValue(_positionSizeLifeTargets[currentPositionSizeLifeTarget]);
            foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _screenQuad.Render(GraphicsDevice);
            }
            _currentPositionSizeLifeTarget = nextPositionSizeLifeTarget;
            GraphicsDevice.SetRenderTarget(null);
        }

        int count = 0;
        private void Draw()
        {
            _particleEffect.CurrentTechnique = _particleEffect.Techniques["Draw"];

            GraphicsDevice.SetVertexBuffer(_particleDrawVertices);
            GraphicsDevice.Indices = _particleDrawIndices;
            _particleEffect.Parameters["PositionSizeRotation"].SetValue(_positionSizeLifeTargets[_currentPositionSizeLifeTarget]);
                HalfVector4[] dat = new HalfVector4[_particleBufferSize * _particleBufferSize];
                for(int i = 0; i < dat.Length; i++)
                {
                    dat[i] = new HalfVector4();
                }
                _positionSizeLifeTargets[_currentPositionSizeLifeTarget].GetData(dat);
            foreach (EffectPass pass in _particleEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList,
                    0,
                    0,
                    _particleCount * 2);
            }
        }

        private void ClearTarget(RenderTarget2D target)
        {
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.SetRenderTarget(null);
        }
    }
}
