using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Components
{
    public class RibbonTrailComponent : IComponent
    {
        public VertexPositionColor[] Verts;
        public int[] Indices;

        public VertexBuffer VertexBuffer = null;
        public IndexBuffer IndexBuffer = null;

        public Color Color = Color.White;

        public byte RenderGroup = 0x1;

        public bool Hidden;

        public RibbonTrailComponent(int trailLength)
        {
            Verts = new VertexPositionColor[trailLength * 2];
            Indices = new int[trailLength * 6 - 6];

            // Initialization
            for(int i = 0; i < Verts.Length; i++)
            {
                Verts[i] = new VertexPositionColor();
            }
        }

        private void InitializeGraphics(GraphicsDevice graphicsDevice)
        {
            VertexBuffer = new VertexBuffer(graphicsDevice,
                typeof(VertexPositionColor),
                Verts.Length,
                BufferUsage.WriteOnly);
            IndexBuffer = new IndexBuffer(graphicsDevice,
                IndexElementSize.ThirtyTwoBits,
                Indices.Length,
                BufferUsage.WriteOnly);
        }
        public void PrepareForRendering(GraphicsDevice graphicsDevice)
        {
            if(VertexBuffer == null
                && IndexBuffer == null)
            {
                InitializeGraphics(graphicsDevice);
            }

            VertexBuffer.SetData(Verts);
            IndexBuffer.SetData(Indices);
        }
    }
}
