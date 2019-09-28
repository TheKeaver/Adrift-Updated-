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
        public float MaxRadius
        {
            get
            {
                return (float)Math.Sqrt(MaxRadiusSquared);
            }
        }

        public Vector2 Offset
        {
            get;
            set;
        }
    }

    public class CircleCollisionShape : CollisionShape
    {
        public float RadiusSquared
        {
            get;
            set;
        }
        public float Radius
        {
            get
            {
                return (float)Math.Sqrt(RadiusSquared);
            }
            set
            {
                RadiusSquared = value * value;
            }
        }

        public override float MaxRadiusSquared
        {
            get
            {
                return RadiusSquared;
            }
        }

        public CircleCollisionShape(float radius)
        {
            Radius = radius;
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

                CalculateMaxRadiusSquared();
            }
        }

        float _maxRadiusSquared;
        public override float MaxRadiusSquared {
            get
            {
                return _maxRadiusSquared;
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

        private void CalculateMaxRadiusSquared()
        {
            _maxRadiusSquared = _vertices[0].LengthSquared();
            for(int i = 1; i < _vertices.Length; i++)
            {
                if(_vertices[i].LengthSquared() > _maxRadiusSquared)
                {
                    _maxRadiusSquared = _vertices[i].LengthSquared();
                }
            }
        }
    }
}
