using System;
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

        public float Alpha = 1;

        public bool Hidden;

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
    }

    public abstract class RenderShape
    {
        public abstract VertexPositionColor[] ComputeVertices();
        public Color TintColor
        {
            get;
            set;
        } = Color.White;
    }

    public class QuadRenderShape : RenderShape
    {
        VertexPositionColor[] _verts;

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
                ComputeVertices();
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
                ComputeVertices();
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
                ComputeVertices();
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
                ComputeVertices();
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
                ComputeVertices();
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

        public override VertexPositionColor[] ComputeVertices()
        {
            return _verts;
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
                    new VertexPositionColor(new Vector3(_v1, 0), _color),
                    new VertexPositionColor(new Vector3(_v2, 0), _color),
                    new VertexPositionColor(new Vector3(_v3, 0), _color),

                    new VertexPositionColor(new Vector3(_v1, 0), _color),
                    new VertexPositionColor(new Vector3(_v3, 0), _color),
                    new VertexPositionColor(new Vector3(_v4, 0), _color),

                    // Top Feather
                    new VertexPositionColor(new Vector3(v1f, 0), featherColor),
                    new VertexPositionColor(new Vector3(v2f, 0), featherColor),
                    new VertexPositionColor(new Vector3(V2, 0), _color),

                    new VertexPositionColor(new Vector3(v1f, 0), featherColor),
                    new VertexPositionColor(new Vector3(V2, 0), _color),
                    new VertexPositionColor(new Vector3(V1, 0), _color),

                    // Bottom Feather
                    new VertexPositionColor(new Vector3(V4, 0), _color),
                    new VertexPositionColor(new Vector3(V3, 0), _color),
                    new VertexPositionColor(new Vector3(v3f, 0), featherColor),

                    new VertexPositionColor(new Vector3(V4, 0), _color),
                    new VertexPositionColor(new Vector3(v3f, 0), featherColor),
                    new VertexPositionColor(new Vector3(v4f, 0), featherColor),

                    // Left Feather
                    new VertexPositionColor(new Vector3(v1f, 0), featherColor),
                    new VertexPositionColor(new Vector3(V1, 0), _color),
                    new VertexPositionColor(new Vector3(V4, 0), _color),

                    new VertexPositionColor(new Vector3(v1f, 0), featherColor),
                    new VertexPositionColor(new Vector3(V4, 0), _color),
                    new VertexPositionColor(new Vector3(v4f, 0), featherColor),

                    // Right Feather
                    new VertexPositionColor(new Vector3(V2, 0), _color),
                    new VertexPositionColor(new Vector3(v2f, 0), featherColor),
                    new VertexPositionColor(new Vector3(v3f, 0), featherColor),

                    new VertexPositionColor(new Vector3(V2, 0), _color),
                    new VertexPositionColor(new Vector3(v3f, 0), featherColor),
                    new VertexPositionColor(new Vector3(V3, 0), _color)
                };
            }
            else
            {
                _verts = new VertexPositionColor[] {
                    new VertexPositionColor(new Vector3(_v1, 0), _color),
                    new VertexPositionColor(new Vector3(_v2, 0), _color),
                    new VertexPositionColor(new Vector3(_v3, 0), _color),

                    new VertexPositionColor(new Vector3(_v1, 0), _color),
                    new VertexPositionColor(new Vector3(_v3, 0), _color),
                    new VertexPositionColor(new Vector3(_v4, 0), _color)
                };
            }
        }
    }

    public class PolyRenderShape : RenderShape
    {
        public enum PolyCapStyle
        {
            None,
            AwayFromCenter,
            Filled
        }

        private VertexPositionColor[] _verts;

        public static PolyRenderShape GenerateCircleRenderShape(float thickness, float radius, Color color, int resolution)
        {
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
        private void Init(Vector2[] points, float thickness, Color[] colors, PolyCapStyle polyCapStyle = PolyCapStyle.None, bool closed = false) {
            bool feathering = CVars.Get<bool>("graphics_feathering");
            float feather = CVars.Get<float>("graphics_feathering_width");

            int count = points.Length;
            if(closed)
            {
                count++;
            }
            switch(polyCapStyle)
            {
                case PolyCapStyle.Filled:
                    _verts = new VertexPositionColor[(count - 1) * (feathering ? 27 : 9)];
                    break;
                case PolyCapStyle.AwayFromCenter:
                default:
                    _verts = new VertexPositionColor[(count - 1) * (feathering ? 18 : 6)];
                    break;
            }
            int v = 0;
            for(int i = 1; i < points.Length + (closed ? 1 : 0); i++)
            {
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

                    if (MathHelper.WrapAngle((float)(Math.Atan2(p2.Y, p2.X) - Math.Atan2(p1.Y, p1.X))) > 0)
                    {
                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);

                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);
                    }
                    else
                    {
                        _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);

                        _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);
                    }
                    continue;
                }

                { // Empty
                    // These are outside because they are needed for end cap feathering
                    Vector2 v2b = p2 - d * thickness / 2;
                    Vector2 v2t = p2 + d * thickness / 2;
                    Vector2 v1b = p1 - d * thickness / 2;
                    Vector2 v1t = p1 + d * thickness / 2;

                    _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                    _verts[v++] = new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1);
                    _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);

                    _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);
                    _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                    _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);

                    Vector2 v2tf = v2t + d * feather;
                    Vector2 v2bf = v2b - d * feather;
                    Color featherColor1 = new Color(c1, 0);
                    Color featherColor2 = new Color(c2, 0);
                    //Color featherColor = Color.HotPink;
                    if (feathering)
                    {
                        Vector2 v1tf = v1t + d * feather;
                        Vector2 v1bf = v1b - d * feather;

                        // Top feather
                        _verts[v++] = new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2tf.X, v2tf.Y, 0), featherColor2);

                        _verts[v++] = new VertexPositionColor(new Vector3(v1t.X, v1t.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2tf.X, v2tf.Y, 0), featherColor2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1tf.X, v1tf.Y, 0), featherColor1);

                        // Bottom feather
                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2bf.X, v2bf.Y, 0), featherColor2);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);

                        _verts[v++] = new VertexPositionColor(new Vector3(v1b.X, v1b.Y, 0), c1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v1bf.X, v1bf.Y, 0), featherColor1);
                        _verts[v++] = new VertexPositionColor(new Vector3(v2bf.X, v2bf.Y, 0), featherColor2);
                    }

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
                        if (Vector2.Dot(top, p2p1) > 0)
                        {
                            _verts[v++] = new VertexPositionColor(new Vector3(v3t.X, v3t.Y, 0), c3);
                            _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                            _verts[v++] = new VertexPositionColor(new Vector3(midpoint.X, midpoint.Y, 0), c23);
                            if (feathering)
                            {
                                Vector2 v3tf = v3t + d2 * feather;

                                _verts[v++] = new VertexPositionColor(new Vector3(v3tf.X, v3tf.Y, 0), featherColor3);
                                _verts[v++] = new VertexPositionColor(new Vector3(v2tf.X, v2tf.Y, 0), featherColor2);
                                _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);

                                _verts[v++] = new VertexPositionColor(new Vector3(v3tf.X, v3tf.Y, 0), featherColor3);
                                _verts[v++] = new VertexPositionColor(new Vector3(v2t.X, v2t.Y, 0), c2);
                                _verts[v++] = new VertexPositionColor(new Vector3(v3t.X, v3t.Y, 0), c3);
                            }
                        } else
                        {
                            _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);
                            _verts[v++] = new VertexPositionColor(new Vector3(v3b.X, v3b.Y, 0), c3);
                            _verts[v++] = new VertexPositionColor(new Vector3(midpoint.X, midpoint.Y, 0), c23);

                            if (feathering)
                            {
                                Vector2 v3bf = v3b - d2 * feather;

                                _verts[v++] = new VertexPositionColor(new Vector3(v2bf.X, v2bf.Y, 0), featherColor2);
                                _verts[v++] = new VertexPositionColor(new Vector3(v3bf.X, v3bf.Y, 0), featherColor3);
                                _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);

                                _verts[v++] = new VertexPositionColor(new Vector3(v3bf.X, v3bf.Y, 0), featherColor3);
                                _verts[v++] = new VertexPositionColor(new Vector3(v3b.X, v3b.Y, 0), c3);
                                _verts[v++] = new VertexPositionColor(new Vector3(v2b.X, v2b.Y, 0), c2);
                            }
                        }
                    }
                }
            }
        }

        public override VertexPositionColor[] ComputeVertices()
        {
            return _verts;
        }
    }
}
