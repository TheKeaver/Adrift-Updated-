using GameJam.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Particles
{
    struct VertexPositionIDVertexID : IVertexType
    {
        public uint ID;
        public uint VertexID;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Byte4, VertexElementUsage.Sample, 0),
            new VertexElement(sizeof(byte) * 4, VertexElementFormat.Byte4, VertexElementUsage.Sample, 1));

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexPositionIDVertexID(uint id, uint vertexID)
        {
            ID = id;
            VertexID = vertexID;
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
        private RenderTarget2D _positionSizeLifeTarget;

        private Effect _particleUpdateEffect;
        private Effect _particleDrawEffect;

        public GPUParticleManager(GraphicsDevice graphicsDevice, ContentManager content, string particleUpdateEffectCVar, string particleDrawEffectCVar)
        {
            GraphicsDevice = graphicsDevice;

            _screenQuad = new Quad();
            _positionSizeLifeTarget = new RenderTarget2D(graphicsDevice,
                1024,
                1024,
                false,
                SurfaceFormat.Color,
                DepthFormat.None,
                0,
                RenderTargetUsage.PreserveContents);
            ClearTarget(_positionSizeLifeTarget);

            _particleUpdateEffect = content.Load<Effect>(particleUpdateEffectCVar);
            _particleDrawEffect = content.Load<Effect>(particleDrawEffectCVar);
            _particleDrawEffect.CurrentTechnique = _particleDrawEffect.Techniques["ParticleDraw"];

            _particleDrawEffect.Parameters["positionSizeLifeWidth"].SetValue(1024);
            _particleDrawEffect.Parameters["positionSizeLifeHeight"].SetValue(1024);
        }

        public void UpdateAndDraw()
        {
            Update();
            Draw();
        }

        private void Update()
        {
            GraphicsDevice.SetRenderTarget(_positionSizeLifeTarget);
            foreach (EffectPass pass in _particleUpdateEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                _screenQuad.Render(GraphicsDevice);
            }
            GraphicsDevice.SetRenderTarget(null);
        }
        private void Draw()
        {
            VertexPositionIDVertexID[] verts = new VertexPositionIDVertexID[]
            {
                new VertexPositionIDVertexID(0, 0),
                new VertexPositionIDVertexID(0, 1),
                new VertexPositionIDVertexID(0, 2),
                new VertexPositionIDVertexID(0, 3)
            };
            int[] indices = new int[]
            {
                0, 1, 2,
                0, 2, 3
            };

            _particleDrawEffect.Parameters["PositionSizeLife"].SetValue(_positionSizeLifeTarget);

            foreach (EffectPass pass in _particleDrawEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList,
                    verts,
                    0,
                    verts.Length,
                    indices,
                    0,
                    indices.Length / 3);
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
