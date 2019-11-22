using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Events;
using GameJam.Events.GameLogic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameJam.Systems
{
    /**
     * SAT - Separated Axis Theorem
     * Notes:
     *
     * SAT, essentially: For convex polygons, if you can draw a line in-between
     * the shapes, they are not intersecting.
     *
     * An AABB is actually a special case of SAT. For an AABB, comparing each
     * edge to each other along different axis (top, bottom, left, right)
     * determines whether the AABB is intersecting or not. SAT is similar,
     * however is generalized to 1) work along any axis (any rotation of the
     * polygon) and 2) work with any number of edges that form a convex polygon.
     *
     * SAT only works with convex polygons, however concave polygons can be
     * split into multiple covex polygons.
     *
     * Steps for SAT:
     *
     * 1) Find the normal of an edge.
     * 2) Find the shadow of all points of each polygon along the normal axis.
     * 3) Find the extent of each polygon (two points that have maximum
     *    distance between them).
     * 4) If the segments do not overlap, the polygons are not intesecting (end).
     * 5) Repeat for all edges of both polygons. If the SAT does not fail,
     *    the polygons must be intersecting.
     **/

    public class CollisionDetectionSystem : BaseSystem
    {
        readonly Family _collisionFamily = Family.All(typeof(CollisionComponent), typeof(TransformComponent)).Get();
        readonly ImmutableList<Entity> _collisionEntities;

        public CollisionDetectionSystem(Engine engine) : base(engine)
        {
            _collisionEntities = engine.GetEntitiesFor(_collisionFamily);
        }

        public override void Update(float dt)
        {
            ProcessCollisions();
        }

        private void ProcessCollisions()
        {
            Dictionary<Entity, List<Entity>> processedPairs = new Dictionary<Entity, List<Entity>>();

            foreach (Entity entityA in _collisionEntities)
            {
                if(!processedPairs.ContainsKey(entityA))
                {
                    processedPairs.Add(entityA, new List<Entity>());
                }

                TransformComponent transformCompA = entityA.GetComponent<TransformComponent>();
                CollisionComponent collisionCompA = entityA.GetComponent<CollisionComponent>();

                float cosA = (float)Math.Cos(transformCompA.Rotation),
                    sinA = (float)Math.Sin(transformCompA.Rotation);

                foreach (Entity entityB in _collisionEntities)
                {
                    if (entityA == entityB)
                    {
                        continue;
                    }
                    if (processedPairs[entityA].Contains(entityB))
                    {
                        continue;
                    }
                    if (!processedPairs.ContainsKey(entityB))
                    {
                        processedPairs.Add(entityB, new List<Entity>());
                    }
                    // Add it to entityB's list because A-B won't occur again.
                    // B-A might.
                    processedPairs[entityB].Add(entityA);

                    TransformComponent transformCompB = entityB.GetComponent<TransformComponent>();
                    CollisionComponent collisionCompB = entityB.GetComponent<CollisionComponent>();

                    float cosB = (float)Math.Cos(transformCompB.Rotation),
                        sinB = (float)Math.Sin(transformCompB.Rotation);


                    bool resolved = false; // If true, _any_ of the shapes have intersected
                    bool intersecting = false;
                    foreach(CollisionShape shapeA in collisionCompA.CollisionShapes)
                    {
                        Vector2 pA = transformCompA.Position +
                            new Vector2(shapeA.Offset.X * cosA - shapeA.Offset.Y * sinA,
                                shapeA.Offset.X * sinB + shapeA.Offset.Y * cosB)
                                * transformCompA.Scale;

                        foreach (CollisionShape shapeB in collisionCompB.CollisionShapes)
                        {
                            Vector2 pB = transformCompB.Position +
                                new Vector2(shapeB.Offset.X * cosB - shapeB.Offset.Y * sinB,
                                    shapeB.Offset.X * sinB + shapeB.Offset.Y * cosB)
                                    * transformCompB.Scale;

                            if ((pB - pA).LengthSquared()
                                <= shapeA.MaxRadiusSquared * transformCompA.Scale * transformCompA.Scale
                                    + shapeB.MaxRadiusSquared * transformCompB.Scale * transformCompB.Scale)
                            {
                                // _May_ be intersecting
                                if(CheckShapeIntersection(pA, cosA, sinA, transformCompA.Scale, shapeA,
                                    pB, cosB, sinB, transformCompB.Scale, shapeB))
                                {
                                    intersecting = true;
                                    resolved = true;
                                    break;
                                }
                            } else
                            {
                                // Guarenteed to not be intersecting
                                continue;
                            }
                        }

                        if(resolved)
                        {
                            break;
                        }
                    }

                    bool previouslyIntersecting = collisionCompA.CollidingWith.Contains(entityB)
                        || collisionCompB.CollidingWith.Contains(entityA);
                    if(!previouslyIntersecting && intersecting)
                    {
                        EventManager.Instance.QueueEvent(new CollisionStartEvent(entityA, entityB));
                        collisionCompA.CollidingWith.Add(entityB);
                        collisionCompB.CollidingWith.Add(entityA);
                    }
                    if(previouslyIntersecting && !intersecting)
                    {
                        EventManager.Instance.QueueEvent(new CollisionEndEvent(entityA, entityB));
                        collisionCompA.CollidingWith.Remove(entityB);
                        collisionCompB.CollidingWith.Remove(entityA);
                    }
                }
            }
        }

        private bool CheckShapeIntersection(Vector2 posA, float cosA, float sinA, float scaleA, CollisionShape shapeA,
            Vector2 posB, float cosB, float sinB, float scaleB, CollisionShape shapeB)
        {
            CircleCollisionShape circleA = shapeA as CircleCollisionShape;
            PolygonCollisionShape polygonA = shapeA as PolygonCollisionShape;

            CircleCollisionShape circleB = shapeB as CircleCollisionShape;
            PolygonCollisionShape polygonB = shapeB as PolygonCollisionShape;

            if(circleA != null && circleB != null)
            {
                return CheckCircleCircleIntersection(posA, circleA,
                    posB, circleB);
            }

            if (circleA != null && polygonB != null)
            {
                return CheckPolygonCircleIntersection(posA, cosA, sinA, scaleA, polygonA,
                    posB, circleB);
            }
            if (circleA != null && polygonB != null)
            {
                return CheckPolygonCircleIntersection(posB, cosB, sinB, scaleB, polygonB,
                    posA, circleA);
            }

            if (polygonA != null && polygonB != null)
            {
                return CheckPolygonPolygonIntersection(posA, cosA, sinA, scaleA, polygonA,
                    posB, cosB, sinB, scaleB, polygonB);
            }

            return false;
        }

        private bool CheckCircleCircleIntersection(Vector2 posA, CircleCollisionShape circleA,
            Vector2 posB, CircleCollisionShape circleB)
        {
            // Special case. Initial check is done using circle collision, which
            // for circle shapes is themselves. This means the check has already
            // been performed and passed. The circles are intersecting.

            // If the initial check is changed (say, to AABB) then this will no
            // longer be true.
            return true;
        }
        private bool CheckPolygonCircleIntersection(Vector2 posA, float cosA, float sinA, float scaleA, PolygonCollisionShape polygonA,
            Vector2 posB, CircleCollisionShape circleB)
        {
            Vector2[] worldPolyA = new Vector2[polygonA.Vertices.Length];
            for (int i = 0; i < polygonA.Vertices.Length; i++)
            {
                worldPolyA[i] = new Vector2(cosA * polygonA.Vertices[i].X - sinA * polygonA.Vertices[i].Y,
                    sinA * polygonA.Vertices[i].X + cosA * polygonA.Vertices[i].Y) * scaleA + posA;
            }

            BoundingCircle worldCircleB = new BoundingCircle(posB, circleB.Radius);

            for(int i = 0; i < worldPolyA.Length; i++)
            {
                int j = (i + 1) % worldPolyA.Length;

                Vector2 v1 = worldPolyA[j];
                Vector2 v2 = worldPolyA[i];

                // Check vertices; guarenteed to intersect if vertices
                // are contained and less expensive than a segment intersection
                // test.
                if(worldCircleB.Contains(v1) || worldCircleB.Contains(v2))
                {
                    return true;
                }

                // Cast circle position onto segment
                Vector2 segment = v2 - v1;
                float circleOnSegment = Vector2.Dot(segment, worldCircleB.Position);
                // Make sure segment is along the segment
                if(circleOnSegment < 0)
                {
                    continue;
                }
                if(circleOnSegment * circleOnSegment > segment.LengthSquared())
                {
                    continue;
                }
                // Find point along segment
                segment.Normalize();
                Vector2 intersectionPoint = segment * circleOnSegment + v1;
                // Check for intersection of intersection point
                if(worldCircleB.Contains(intersectionPoint))
                {
                    return true;
                }
            }

            return false;
        }
        private bool CheckPolygonPolygonIntersection(Vector2 posA, float cosA, float sinA, float scaleA, PolygonCollisionShape polygonA,
            Vector2 posB, float cosB, float sinB, float scaleB, PolygonCollisionShape polygonB)
        {
            Vector2[] worldPolyA = new Vector2[polygonA.Vertices.Length];
            for (int i = 0; i < polygonA.Vertices.Length; i++)
            {
                worldPolyA[i] = new Vector2(cosA * polygonA.Vertices[i].X - sinA * polygonA.Vertices[i].Y,
                    sinA * polygonA.Vertices[i].X + cosA * polygonA.Vertices[i].Y) * scaleA + posA;
            }
            Vector2[] worldPolyB = new Vector2[polygonB.Vertices.Length];
            for (int i = 0; i < polygonB.Vertices.Length; i++)
            {
                worldPolyB[i] = new Vector2(cosB * polygonB.Vertices[i].X - sinB * polygonB.Vertices[i].Y,
                    sinB * polygonB.Vertices[i].X + cosB * polygonB.Vertices[i].Y) * scaleB + posB;
            }

            for(int i = 0; i < worldPolyA.Length; i++)
            {
                int j = (i + 1) % worldPolyA.Length;
                Vector2 edge = worldPolyA[j] - worldPolyA[i];

                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                if(!PassSAT(worldPolyA, worldPolyB, axis))
                {
                    // SAT: If any check does _not_ pass, they are _not_ intersecting
                    return false;
                }
            }
            for (int i = 0; i < worldPolyB.Length; i++)
            {
                int j = (i + 1) % worldPolyB.Length;
                Vector2 edge = worldPolyB[j] - worldPolyB[i];

                Vector2 axis = new Vector2(-edge.Y, edge.X);
                axis.Normalize();

                if (!PassSAT(worldPolyA, worldPolyB, axis))
                {
                    // SAT: If any check does _not_ pass, they are _not_ intersecting
                    return false;
                }
            }

            return true;
        }
        private bool PassSAT(Vector2[] polyA, Vector2[] polyB, Vector2 axis)
        {
            float minA = float.PositiveInfinity, maxA = float.NegativeInfinity;
            for(int i = 0; i < polyA.Length; i++)
            {
                float q = Vector2.Dot(polyA[i], axis);
                minA = Math.Min(minA, q);
                maxA = Math.Max(maxA, q);
            }

            float minB = float.PositiveInfinity, maxB = float.NegativeInfinity;
            for (int i = 0; i < polyB.Length; i++)
            {
                float q = Vector2.Dot(polyB[i], axis);
                minB = Math.Min(minB, q);
                maxB = Math.Max(maxB, q);
            }

            return maxB >= minA && maxA >= minB;
        }
    }
}
