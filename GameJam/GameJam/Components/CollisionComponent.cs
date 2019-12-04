using Audrey;
using GameJam.Common;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameJam.Components
{
    public class CollisionComponent : IComponent
    {
        public List<CollisionShape> CollisionShapes;
        public List<Entity> CollidingWith = new List<Entity>();
        public byte CollisionGroup = Constants.Collision.GROUP_ONE;
        public byte CollisionMask = Constants.Collision.GROUP_MASK_ALL;

        public CollisionComponent(IEnumerable<CollisionShape> collisionShapes)
        {
            CollisionShapes = new List<CollisionShape>(collisionShapes);
        }
        public CollisionComponent(CollisionShape collisionShape)
        {
            CollisionShapes = new List<CollisionShape>();
            CollisionShapes.Add(collisionShape);
        }
    }

    public abstract class CollisionShape
    {
        public abstract float MaxRadiusSquared
        {
            get;
        }
        public abstract float MaxRadius
        {
            get;
        }

        public Vector2 Offset
        {
            get;
            set;
        }

        public abstract BoundingRect GetAABB(float cos, float sin, float scale);
    }

    public class CircleCollisionShape : CollisionShape
    {
        private float _radiusSquared;
        public float RadiusSquared
        {
            get
            {
                return _radiusSquared;
            }
            set
            {
                _radiusSquared = value;
                _radius = (float)Math.Sqrt(value);
            }
        }
        private float _radius;
        public float Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                _radiusSquared = value * value;
            }
        }

        public override float MaxRadiusSquared
        {
            get
            {
                return RadiusSquared;
            }
        }
        public override float MaxRadius
        {
            get
            {
                return Radius;
            }
        }

        public CircleCollisionShape(float radius)
        {
            Radius = radius;
        }

        public override BoundingRect GetAABB(float cos, float sin, float scale)
        {
            return new BoundingRect(Offset.X - Radius / 2, Offset.Y - Radius / 2,
                Radius, Radius);
        }
    }

    public class PolygonCollisionShape : CollisionShape
    {
        private Vector2[] _vertices;
        public Vector2[] Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;

                CalculateMaxRadii();
            }
        }

        float _maxRadiusSquared;
        public override float MaxRadiusSquared {
            get
            {
                return _maxRadiusSquared;
            }
        }
        float _maxRadius;
        public override float MaxRadius
        {
            get
            {
                return _maxRadius;
            }
        }

        public bool AxisAligned
        {
            get;
            set;
        } = false;

        public PolygonCollisionShape(Vector2[] vertices)
        {
            Vertices = vertices;
        }

        private void CalculateMaxRadii()
        {
            _maxRadiusSquared = _vertices[0].LengthSquared();
            for(int i = 1; i < _vertices.Length; i++)
            {
                if(_vertices[i].LengthSquared() > _maxRadiusSquared)
                {
                    _maxRadiusSquared = _vertices[i].LengthSquared();
                }
            }

            _maxRadius = (float)Math.Sqrt(_maxRadiusSquared);
        }

        public override BoundingRect GetAABB(float cos, float sin, float scale)
        {
            float minX = float.PositiveInfinity, minY = float.PositiveInfinity,
                maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

            Vector2 transformedOffset = new Vector2(Offset.X * cos - Offset.Y * sin,
                                        Offset.X * sin + Offset.Y * cos);

            for (int i = 0; i < _vertices.Length; i++)
            {
                Vector2 transformedVertex = (new Vector2(_vertices[i].X * cos - _vertices[i].Y * sin,
                    _vertices[i].X * sin + _vertices[i].Y * cos)
                    + transformedOffset) * scale;

                if(transformedVertex.X < minX)
                {
                    minX = transformedVertex.X;
                }
                if (transformedVertex.X > maxX)
                {
                    maxX = transformedVertex.X;
                }
                if (transformedVertex.Y < minY)
                {
                    minY = transformedVertex.Y;
                }
                if (transformedVertex.Y > maxY)
                {
                    maxY = transformedVertex.Y;
                }
            }

            float width = maxX - minX;
            float height = maxY - minY;
            return new BoundingRect(transformedOffset.X - width / 2, transformedOffset.Y - height / 2,
                width, height);
        }
    }
}
