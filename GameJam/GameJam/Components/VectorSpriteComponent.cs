using System;
using System.Collections.Generic;
using System.Linq;
using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Components
{
    public class VectorSpriteComponent : IComponent, IRenderComponent
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

        public float Alpha = 1;

        public bool Hidden;

        public float Depth = 0;

        public Vector2 Stretch { get; private set; } = Vector2.One;
        public Vector2 LastStretch { get; private set; } = Vector2.One;
        public void ChangeStretch(Vector2 newStretch, bool reset = false)
        {
            LastStretch = Stretch;
            Stretch = newStretch;
            if (reset)
            {
                LastStretch = Stretch;
            }
        }
        public void ChangeColor(Color color)
        {
            foreach(RenderShape shape in RenderShapes)
            {
                shape.TintColor = color;
            }
        }
        public BoundingRect GetAABB(float scale)
        {
            BoundingRect returnRect = RenderShapes[0].GetAABB(scale);

            for(int i=1; i<RenderShapes.Count; i++)
            {
                BoundingRect temp = RenderShapes[i].GetAABB(scale);
                returnRect = BoundingRect.Union(returnRect, temp);
            }
            return returnRect;
        }

        public bool IsHidden()
        {
            return Hidden;
        }
    }

    public abstract class RenderShape
    {
        public abstract void ComputeVertices(out VertexPositionColor[] verts, out int[] indices);
        public abstract BoundingRect GetAABB(float scale);
        public Color TintColor
        {
            get;
            set;
        } = Color.White;
    }

    public class QuadRenderShape : RenderShape
    {
        private VertexPositionColor[] _verts;
        private int[] _indices;

        private Vector2 _v1;
        private Vector2 _v2;
        private Vector2 _v3;
        private Vector2 _v4;
        private Color _color;

        public Vector2 V1
        {
            get
            {
                return _v1;
            }
            set
            {
                _v1 = value;
                RebuildVerts();
            }
        }
        public Vector2 V2
        {
            get
            {
                return _v2;
            }
            set
            {
                _v2 = value;
                RebuildVerts();
            }
        }
        public Vector2 V3
        {
            get
            {
                return _v3;
            }
            set
            {
                _v3 = value;
                RebuildVerts();
            }
        }
        public Vector2 V4
        {
            get
            {
                return _v4;
            }
            set
            {
                _v4 = value;
                RebuildVerts();
            }
        }
        public Color Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                RebuildVerts();
            }
        }

        public QuadRenderShape(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Color color)
        {
            _v1 = v1;
            _v2 = v2;
            _v3 = v3;
            _v4 = v4;
            _color = color;

            RebuildVerts();
        }

        public override void ComputeVertices(out VertexPositionColor[] verts, out int[] indices)
        {
            verts = _verts;
            indices = _indices;
        }

        private void RebuildVerts()
        {
            bool feathering = CVars.Get<bool>("graphics_feathering");
            if (feathering)
            {
                float feather = CVars.Get<float>("graphics_feathering_width");

                Vector2 v12 = V1 - V2;
                v12 = new Vector2(-v12.Y, v12.X);
                v12.Normalize();
                Vector2 v23 = V2 - V3;
                v23 = new Vector2(-v23.Y, v23.X);
                v23.Normalize();
                Vector2 v34 = V3 - V4;
                v34 = new Vector2(-v34.Y, v34.X);
                v34.Normalize();
                Vector2 v41 = V4 - V1;
                v41 = new Vector2(-v41.Y, v41.X);
                v41.Normalize();

                Vector2 v1Norm = v41 + v12;
                v1Norm.Normalize();
                Vector2 v2Norm = v12 + v23;
                v2Norm.Normalize();
                Vector2 v3Norm = v23 + v34;
                v3Norm.Normalize();
                Vector2 v4Norm = v34 + v41;
                v4Norm.Normalize();

                float diagonalFeather = (float)(feather * Math.Sqrt(2));

                Vector2 v1f = v1Norm * diagonalFeather + V1;
                Vector2 v2f = v2Norm * diagonalFeather + V2;
                Vector2 v3f = v3Norm * diagonalFeather + V3;
                Vector2 v4f = v4Norm * diagonalFeather + V4;

                Color featherColor = new Color(_color, 0);

                _verts = new VertexPositionColor[] {
                    // Body
                    new VertexPositionColor(new Vector3(_v1, 0), _color), // 0
                    new VertexPositionColor(new Vector3(_v2, 0), _color), // 1
                    new VertexPositionColor(new Vector3(_v3, 0), _color), // 2

                    new VertexPositionColor(new Vector3(_v4, 0), _color), // 3
                    
                    // Top Feather
                    new VertexPositionColor(new Vector3(v1f, 0), featherColor), // 4
                    new VertexPositionColor(new Vector3(v2f, 0), featherColor), // 5
                    new VertexPositionColor(new Vector3(V2, 0), _color), // 6

                    new VertexPositionColor(new Vector3(V1, 0), _color), // 7

                    //Bottom Feather
                    new VertexPositionColor(new Vector3(V4, 0), _color), // 8
                    new VertexPositionColor(new Vector3(V3, 0), featherColor), // 9
                    new VertexPositionColor(new Vector3(v3f, 0), featherColor), // 10

                    new VertexPositionColor(new Vector3(v4f, 0), featherColor), // 11

                    // Rest of feathers already in list
                };

                _indices = new int[] {
                    2, 1, 0,
                    3, 2, 0,

                    6, 5, 4,
                    7, 6, 4,

                    10, 9, 8,
                    11, 10, 8,

                    8, 7, 4,
                    11, 8, 4,

                    10, 5, 6,
                    9, 10, 6
                };
            }
            else
            {
                _verts = new VertexPositionColor[]
                {
                    new VertexPositionColor(new Vector3(_v1, 0), _color),
                    new VertexPositionColor(new Vector3(_v2, 0), _color),
                    new VertexPositionColor(new Vector3(_v3, 0), _color),

                    new VertexPositionColor(new Vector3(_v4, 0), _color)
                };
                _indices = new int[]
                {
                    0,1,2,
                    0,2,3
                };
            }
        }

        public override BoundingRect GetAABB(float scale)
        {
            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity),
                max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            for (int i = 0; i < _verts.Length; i++)
            {
                Vector2 transformedVertex = (new Vector2(_verts[i].Position.X - _verts[i].Position.Y,
                    _verts[i].Position.X + _verts[i].Position.Y) * scale);

                if (transformedVertex.X < min.X)
                {
                    min.X = transformedVertex.X;
                }
                if (transformedVertex.X > max.X)
                {
                    max.X = transformedVertex.X;
                }
                if (transformedVertex.Y < min.Y)
                {
                    min.Y = transformedVertex.Y;
                }
                if (transformedVertex.Y > max.Y)
                {
                    max.Y = transformedVertex.Y;
                }
            }

            return new BoundingRect(min, max);
        }
    }

    public class PolyRenderShape : RenderShape
    {
        private List<VertexPositionColor> _verts = new List<VertexPositionColor>();
        private List<int> _indices = new List<int>();

        // Finish implementing abstract methods
        public enum PolyCapStyle
        {
            None,
            AwayFromCenter,
            Filled
        }

        public static PolyRenderShape GenerateCircleRenderShape(float thickness, float radius, Color color, int resolution)
        {
            thickness = (CVars.Get<bool>("graphics_feathering")) ? thickness : 1.5f * thickness;
            Vector2[] points = new Vector2[resolution];

            for(int i = 0; i < points.Length; i++)
            {
                float angle = (MathHelper.TwoPi / resolution) * i;
                points[i] = new Vector2((float)(radius * Math.Cos(angle)), (float)(radius * Math.Sin(angle)));
            }

            return new PolyRenderShape(points, thickness, color, PolyCapStyle.None, true);
        }

        public PolyRenderShape(Vector2[] points, float thickness, Color color, PolyCapStyle polyCapStyle = PolyCapStyle.None, bool closed = false)
        {
            thickness = (CVars.Get<bool>("graphics_feathering")) ? thickness : 1.5f * thickness;
            Color[] colors = new Color[points.Length];
            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = color;
            }
            Init(points, thickness, colors, polyCapStyle, closed);
        }
        public PolyRenderShape(Vector2[] points, float thickness, Color[] colors, PolyCapStyle polyCapStyle = PolyCapStyle.None, bool closed = false)
        {
            Init(points, thickness, colors, polyCapStyle, closed);
        }
        private void Init(Vector2[] points, float thickness, Color[] colors, PolyCapStyle polyCapStyle = PolyCapStyle.None, bool closed = false)
        {
            _verts.Clear();
            _indices.Clear();
            bool feathering = CVars.Get<bool>("graphics_feathering");
            float feather = CVars.Get<float>("graphics_feathering_width");

            thickness = feathering ? thickness : 1.5f * thickness;

            int count = points.Length;
            if(closed)
            {
                count++;
            }
            int vertexCount;
            for (int i = 1; i < points.Length + (closed ? 1 : 0); i++)
            {
                vertexCount = _verts.Count;
                Vector2 p1 = points[i - 1];
                Color c1 = colors[i - 1];
                Vector2 p2;
                Color c2;
                if (i >= points.Length) {
                    p2 = points[0];
                    c2 = colors[0];
                } else
                {
                    p2 = points[i];
                    c2 = colors[i];
                }

                Vector2 p2p1 = p2 - p1;
                Vector2 d = new Vector2(-p2p1.Y, p2p1.X);
                d.Normalize();


                if (polyCapStyle == PolyCapStyle.AwayFromCenter)
                {
                    Vector2 oToP1 = new Vector2(p1.X, p1.Y);
                    oToP1.Normalize();
                    Vector2 oToP2 = new Vector2(p2.X, p2.Y);
                    oToP2.Normalize();

                    float p1Thickness = thickness / Math.Abs(Vector2.Dot(d, oToP1));
                    float p2Thickness = thickness / Math.Abs(Vector2.Dot(d, oToP2));

                    Vector2 v1b = p1 - p1Thickness / 2 * oToP1;
                    Vector2 v1t = p1 + p1Thickness / 2 * oToP1;
                    Vector2 v2b = p2 - p2Thickness / 2 * oToP2;
                    Vector2 v2t = p2 + p2Thickness / 2 * oToP2;

                    _verts.Add(new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1)); // 0
                    _verts.Add(new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1)); // 1
                    _verts.Add(new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2)); // 2
                    _verts.Add(new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2)); // 3

                    if (MathHelper.WrapAngle((float)(Math.Atan2(p2.Y, p2.X) - Math.Atan2(p1.Y, p1.X))) > 0)
                    {
                        _indices.Add(2 + vertexCount);
                        _indices.Add(1 + vertexCount);
                        _indices.Add(0 + vertexCount);

                        _indices.Add(3 + vertexCount);
                        _indices.Add(2 + vertexCount);
                        _indices.Add(0 + vertexCount);
                    }
                    else
                    {
                        _indices.Add(0 + vertexCount);
                        _indices.Add(1 + vertexCount);
                        _indices.Add(2 + vertexCount);

                        _indices.Add(0 + vertexCount);
                        _indices.Add(2 + vertexCount);
                        _indices.Add(3 + vertexCount);
                    }
                    continue;
                }

                { // Empty and Filled
                    // These are outside because they are needed for end cap feathering
                    Vector2 v2b = p2 - d * thickness / 2;
                    Vector2 v2t = p2 + d * thickness / 2;
                    Vector2 v1b = p1 - d * thickness / 2;
                    Vector2 v1t = p1 + d * thickness / 2;

                    _verts.Add(new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2)); // 0
                    _verts.Add(new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1)); // 1
                    _verts.Add(new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1)); // 2
                    _verts.Add(new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2)); // 3

                    _indices.Add(2 + vertexCount);
                    _indices.Add(1 + vertexCount);
                    _indices.Add(0 + vertexCount);

                    _indices.Add(2 + vertexCount);
                    _indices.Add(0 + vertexCount);
                    _indices.Add(3 + vertexCount);

                    Vector2 v2tf = v2t + d * feather;
                    Vector2 v2bf = v2b - d * feather;
                    Color featherColor1 = new Color(c1, 0);
                    Color featherColor2 = new Color(c2, 0);
                    //Color featherColor = Color.HotPink;
                    if (feathering)
                    {
                        Vector2 v1tf = v1t + d * feather;
                        Vector2 v1bf = v1b - d * feather;

                        _verts.Add(new VertexPositionColor(new Vector3(v2tf.X, v2tf.Y, 0), featherColor2)); // 4
                        _verts.Add(new VertexPositionColor(new Vector3(v1tf.X, v1tf.Y, 0), featherColor1)); // 5
                        _verts.Add(new VertexPositionColor(new Vector3(v2bf.X, v2bf.Y, 0), featherColor2)); // 6
                        _verts.Add(new VertexPositionColor(new Vector3(v1bf.X, v1bf.Y, 0), featherColor1)); // 7

                        _indices.Add(4 + vertexCount);
                        _indices.Add(0 + vertexCount);
                        _indices.Add(1 + vertexCount);

                        _indices.Add(5 + vertexCount);
                        _indices.Add(4 + vertexCount);
                        _indices.Add(1 + vertexCount);

                        _indices.Add(3 + vertexCount);
                        _indices.Add(6 + vertexCount);
                        _indices.Add(2 + vertexCount);

                        _indices.Add(6 + vertexCount);
                        _indices.Add(7 + vertexCount);
                        _indices.Add(2 + vertexCount);
                    }
                    // Only Filled
                    if (polyCapStyle == PolyCapStyle.Filled)
                    {
                        int j;
                        if (i >= points.Length)
                        {
                            j = 0;
                        }
                        else
                        {
                            j = i;
                        }
                        Vector2 p3 = points[j];
                        Color c3 = colors[j];
                        Vector2 p4;
                        if(j == points.Length - 1)
                        {
                            if(!closed)
                            {
                                continue;
                            }

                            p4 = points[0];
                        } else
                        {
                            p4 = points[j + 1];
                        }

                        Vector2 p4p3 = p4 - p3;
                        Vector2 d2 = new Vector2(-p4p3.Y, p4p3.X);
                        d2.Normalize();

                        Vector2 v3b = p3 - d2 * thickness / 2;
                        Vector2 v3t = p3 + d2 * thickness / 2;

                        Color featherColor3 = new Color(c3, 0);
                        Color c23;
                        {
                            Vector4 c2_vec = c2.ToVector4();
                            Vector4 c3_vec = c3.ToVector4();
                            Vector4 c23_vec = c2_vec * c2_vec + c3_vec * c3_vec;
                            c23_vec.X = (float)Math.Sqrt(c23_vec.X);
                            c23_vec.Y = (float)Math.Sqrt(c23_vec.Y);
                            c23_vec.Z = (float)Math.Sqrt(c23_vec.Z);
                            c23_vec.W = (float)Math.Sqrt(c23_vec.W);
                            c23 = new Color(c23_vec);
                        }

                        // Check which to fill in - top or bottom
                        Vector2 top = v3t - v2t;
                        Vector2 midpoint = p2;
                        int featheringOffset = (feathering) ? 4 : 0;

                        if (Vector2.Dot(top, p2p1) > 0)
                        {
                            _verts.Add(new VertexPositionColor(new Vector3(v3t.X, v3t.Y, 0), c3)); // 4 or 8
                            _verts.Add(new VertexPositionColor(new Vector3(midpoint.X, midpoint.Y, 0), c23)); // 5 or 9

                            _indices.Add(5 + featheringOffset);
                            _indices.Add(0);
                            _indices.Add(4 + featheringOffset);

                            if (feathering)
                            {
                                Vector2 v3tf = v3t + d2 * feather;

                                _verts.Add(new VertexPositionColor(new Vector3(v3tf.X, v3tf.Y, 0), featherColor3)); // 10

                                _indices.Add(0 + vertexCount);
                                _indices.Add(4 + vertexCount);
                                _indices.Add(10 + vertexCount);

                                _indices.Add(8 + vertexCount);
                                _indices.Add(0 + vertexCount);
                                _indices.Add(10 + vertexCount);
                            }
                        } 
                        else
                        {
                            _verts.Add(new VertexPositionColor(new Vector3(v3b.X, v3b.Y, 0), c3)); // 4 or 8;
                            _verts.Add(new VertexPositionColor(new Vector3(midpoint.X, midpoint.Y, 0), c23)); // 5 or 9;

                            _indices.Add(5 + featheringOffset + vertexCount);
                            _indices.Add(4 + featheringOffset + vertexCount);
                            _indices.Add(3 + vertexCount);

                            if (feathering)
                            {
                                Vector2 v3bf = v3b - d2 * feather;

                                _verts.Add(new VertexPositionColor(new Vector3(v3bf.X, v3bf.Y, 0), featherColor3)); // 10

                                _indices.Add(3 + vertexCount);
                                _indices.Add(10 + vertexCount);
                                _indices.Add(6 + vertexCount);

                                _indices.Add(3 + vertexCount);
                                _indices.Add(8 + vertexCount);
                                _indices.Add(10 + vertexCount);
                            }
                        }
                    }
                }
            }
        }

        public override BoundingRect GetAABB(float scale)
        {
            Vector2 min = new Vector2(float.PositiveInfinity, float.PositiveInfinity),
                max = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

            for (int i=0; i < _verts.Count; i++)
            {
                Vector2 transformedVertex = (new Vector2(_verts[i].Position.X - _verts[i].Position.Y,
                    _verts[i].Position.X + _verts[i].Position.Y) * scale);

                if (transformedVertex.X < min.X)
                {
                    min.X = transformedVertex.X;
                }
                if (transformedVertex.X > max.X)
                {
                    max.X = transformedVertex.X;
                }
                if (transformedVertex.Y < min.Y)
                {
                    min.Y = transformedVertex.Y;
                }
                if (transformedVertex.Y > max.Y)
                {
                    max.Y = transformedVertex.Y;
                }
            }

            return new BoundingRect(min, max);
        }

        public override void ComputeVertices(out VertexPositionColor[] verts, out int[] indices)
        {
            verts = _verts.ToArray();
            indices = _indices.ToArray();
        }
    }
}
