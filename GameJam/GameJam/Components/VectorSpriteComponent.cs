using System.Collections.Generic;
using Audrey;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Components
{
    public class VectorSpriteComponent : IComponent
    {
        public VectorSpriteComponent()
        {
        }
        public VectorSpriteComponent(RenderShape[] renderShapes)
        {
            RenderShapes = new List<RenderShape>(renderShapes);
        }

        public List<RenderShape> RenderShapes = new List<RenderShape>();

        public byte RenderGroup = 0x1;

        public bool Hidden;
    }

    public abstract class RenderShape
    {
        public abstract VertexPositionColor[] ComputeVertices();
    }

    public class TriangleRenderShape : RenderShape
    {
        readonly VertexPositionColor[] _verts;

        public TriangleRenderShape(Vector2 v1, Vector2 v2, Vector2 v3, Color color)
        {
            _verts = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(v1, 0), color),
                new VertexPositionColor(new Vector3(v2, 0), color),
                new VertexPositionColor(new Vector3(v3, 0), color)
            };
        }

        public override VertexPositionColor[] ComputeVertices()
        {
            return _verts;
        }
    }

    public class QuadRenderShape : RenderShape
    {
        readonly VertexPositionColor[] _verts;

        public QuadRenderShape(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color)
        {
            _verts = new VertexPositionColor[] {
                new VertexPositionColor(new Vector3(v1, 0), color),
                new VertexPositionColor(new Vector3(v2, 0), color),
                new VertexPositionColor(new Vector3(v3, 0), color),

                new VertexPositionColor(new Vector3(v1, 0), color),
                new VertexPositionColor(new Vector3(v3, 0), color),
                new VertexPositionColor(new Vector3(v4, 0), color)
            };
        }

        public override VertexPositionColor[] ComputeVertices()
        {
            return _verts;
        }
    }
}
