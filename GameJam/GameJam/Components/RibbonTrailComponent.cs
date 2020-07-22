using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class RibbonTrailComponent : IComponent
    {
        public VertexPositionColor[] Verts;
        public int[] Indices;

        public int TrailLength
        {
            get
            {
                return Verts.Length / 2;
            }
            set
            {
                Verts = new VertexPositionColor[value * 2];
                Indices = new int[value * 6 - 6];

                // Initialization
                for (int i = 0; i < Verts.Length; i++)
                {
                    Verts[i] = new VertexPositionColor();
                }

                if (VertexBuffer != null)
                {
                    VertexBuffer.Dispose();
                    VertexBuffer = null;
                }
                if (IndexBuffer != null)
                {
                    IndexBuffer.Dispose();
                    IndexBuffer = null;
                }
            }
        }

        public List<int> Starts;
        public List<int> Ends;

        public VertexBuffer VertexBuffer = null;
        public IndexBuffer IndexBuffer = null;

        public Color Color = Color.White;

        public byte RenderGroup = 0x1;

        public bool Hidden;

        public RibbonTrailComponent(int trailLength)
        {
            TrailLength = trailLength;

            Starts = new List<int>();
            Starts.Add(0);
            Starts.Add(40);
            Ends = new List<int>();
            Ends.Add(20);
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
