using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameJam.Particles
{
    struct VertexIDVertexID : IVertexType
    {
        public uint ID;
        public uint VertexID;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(byte) * 4, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1));

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexID(uint id, uint vertexID)
        {
            ID = id;
            VertexID = vertexID;
        }
    }
    struct VertexIDVertexIDPosition : IVertexType
    {
        public uint ID;
        public uint VertexID;
        public Vector2 Position;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(byte) * 4, VertexElementFormat.Byte4, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(sizeof(byte) * 8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2)
            );
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexIDVertexIDPosition(uint id, uint vertexID, Vector2 position)
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

        private Effect _particleCreateEffect;
        private Effect _particleUpdateEffect;
        private Effect _particleDrawEffect;

        private VertexBuffer _particleDrawVertices;
        private IndexBuffer _particleDrawIndices;

        private List<VertexIDVertexIDPosition> _particleCreateVertices = new List<VertexIDVertexIDPosition>();
        private List<int> _particleCreateIndices = new List<int>();

        private float _elapsedTime;

        private int _particleCount = 10;
        private int _particleBufferSize = 1024;

        private int _nextParticleID = 0;

        public GPUParticleManager(GraphicsDevice graphicsDevice, ContentManager content, string particleCreateEffectCVar, string particleUpdateEffectCVar, string particleDrawEffectCVar)
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

            _particleCreateEffect = content.Load<Effect>(particleCreateEffectCVar);
            //_particleCreateEffect.Parameters["Size"].SetValue(_particleBufferSize);
            _particleUpdateEffect = content.Load<Effect>(particleUpdateEffectCVar);
            _particleDrawEffect = content.Load<Effect>(particleDrawEffectCVar);
            _particleDrawEffect.Parameters["Size"].SetValue(_particleBufferSize);

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

            {
                CreateParticle(new Vector2(-0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
                //CreateParticle(new Vector2(0.5f, 0));
            }
        }

        public void CreateParticle(Vector2 position)
        {
            int start = _particleCreateVertices.Count;

            _particleCreateVertices.Add(new VertexIDVertexIDPosition((uint)_nextParticleID,
                0,
                position));
            _particleCreateVertices.Add(new VertexIDVertexIDPosition((uint)_nextParticleID,
                1,
                position));
            _particleCreateVertices.Add(new VertexIDVertexIDPosition((uint)_nextParticleID,
                2,
                position));
            _particleCreateVertices.Add(new VertexIDVertexIDPosition((uint)_nextParticleID,
                3,
                position));

            _particleCreateIndices.Add(start);
            _particleCreateIndices.Add(start + 1);
            _particleCreateIndices.Add(start + 2);

            _particleCreateIndices.Add(start);
            _particleCreateIndices.Add(start + 2);
            _particleCreateIndices.Add(start + 3);

            _nextParticleID = (_nextParticleID + 1) % _particleCount;
        }
        
        private void UploadParticleDrawVertices()
        {
            VertexIDVertexID[] verts = new VertexIDVertexID[4 * _particleCount];
            int i = 0;
            for(uint j = 0; j < _particleCount; j++)
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

            Create();
            Update();
            Draw();
        }

        private void Create()
        {
            if(_particleCreateVertices.Count > 0 && _particleCreateIndices.Count > 0)
            {
                GraphicsDevice.SetRenderTarget(_positionSizeLifeTargets[_currentPositionSizeLifeTarget]);
                foreach(EffectPass pass in _particleCreateEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();

                    GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                        _particleCreateVertices.ToArray(),
                        0,
                        _particleCreateVertices.Count,
                        _particleCreateIndices.ToArray(),
                        0,
                        _particleCreateIndices.Count / 6);
                }
                GraphicsDevice.SetRenderTarget(null);

                _particleCreateVertices.Clear();
                _particleCreateIndices.Clear();
            }
        }
        private void Update()
        {
            int currentPositionSizeLifeTarget = _currentPositionSizeLifeTarget;
            int nextPositionSizeLifeTarget = (_currentPositionSizeLifeTarget + 1) % _positionSizeLifeTargets.Length;

            GraphicsDevice.SetRenderTarget(_positionSizeLifeTargets[nextPositionSizeLifeTarget]);
            GraphicsDevice.Clear(Color.TransparentBlack);
            //_particleUpdateEffect.Parameters["ElapsedTime"].SetValue(_elapsedTime);
            _particleUpdateEffect.Parameters["PositionSizeLife"].SetValue(_positionSizeLifeTargets[currentPositionSizeLifeTarget]);
            foreach (EffectPass pass in _particleUpdateEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _screenQuad.Render(GraphicsDevice);
            }
            _currentPositionSizeLifeTarget = nextPositionSizeLifeTarget;
            GraphicsDevice.SetRenderTarget(null);
        }
        private void Draw()
        {
            GraphicsDevice.SetVertexBuffer(_particleDrawVertices);
            GraphicsDevice.Indices = _particleDrawIndices;

            _particleDrawEffect.Parameters["PositionSizeLife"].SetValue(_positionSizeLifeTargets[_currentPositionSizeLifeTarget]);

            foreach (EffectPass pass in _particleDrawEffect.CurrentTechnique.Passes)
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
