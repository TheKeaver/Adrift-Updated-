using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameJam.Systems
{
    class EnemySeparationSystem : BaseSystem
    {
        readonly Family _movingEnemyFamily = Family.All(typeof(MovementComponent), typeof(EnemyComponent), typeof(QuadTreeReferenceComponent), typeof(TransformComponent))
            .Exclude(typeof(ProjectileComponent), typeof(LaserBeamComponent), typeof(LaserBeamReflectionComponent), typeof(GravityHoleEnemyComponent)).Get();

        readonly ImmutableList<Entity> _movingEnemyEntities;

        public EnemySeparationSystem(Engine engine) : base(engine)
        {
            _movingEnemyEntities = Engine.GetEntitiesFor(_movingEnemyFamily);
        }

        public void Update(float dt)
        {
            Dictionary<Entity, List<Entity>> correctedEntityPairs = new Dictionary<Entity, List<Entity>>();

            foreach(Entity entity in _movingEnemyEntities)
            {
                QuadTreeReferenceComponent quadTreeReferenceComp = entity.GetComponent<QuadTreeReferenceComponent>();
                if(quadTreeReferenceComp.node == null)
                {
                    // Quad tree hasn't been generated yet (or this entity isn't a part of it yet),
                    // skip.
                    continue;
                }
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                QuadTreeNode root = quadTreeReferenceComp.node;
                while(root.parent != null)
                {
                    root = root.parent;
                }

                // Find all quad tree nodes that intersect the radius we are searching for
                float radius = CVars.Get<float>("enemy_minimum_separation_distance");
                float radiusSquared = radius * radius;
                List<QuadTreeNode> intersectingNodes = new List<QuadTreeNode>();
                InsertIntersectingChildNodes(transformComp.Position, radius, quadTreeReferenceComp.node, root, ref intersectingNodes);

                List<Entity> neighborEntities = new List<Entity>();
                foreach(QuadTreeNode node in intersectingNodes)
                {
                    foreach(Entity otherEntity in node.leaves)
                    {
                        if(otherEntity == entity)
                        {
                            continue;
                        }

                        if(_movingEnemyFamily.Matches(otherEntity))
                        {
                            TransformComponent otherTransformComp = otherEntity.GetComponent<TransformComponent>();
                            if((transformComp.Position - otherTransformComp.Position).LengthSquared() <= radiusSquared)
                            {
                                neighborEntities.Add(otherEntity);
                            }
                        }
                    }
                }

                if (neighborEntities.Count > 0)
                {
                    foreach(Entity otherEntity in neighborEntities)
                    {
                        if(!correctedEntityPairs.ContainsKey(entity))
                        {
                            correctedEntityPairs.Add(entity, new List<Entity>());
                        }
                        if(correctedEntityPairs[entity].Contains(otherEntity))
                        {
                            // This entity pair has already been separated, skip
                            continue;
                        }

                        SeparateEntities(entity, otherEntity, radius);

                        // Add to other's list, since this Entity is this current loop (can't loop this entity again this frame)
                        if (!correctedEntityPairs.ContainsKey(otherEntity))
                        {
                            correctedEntityPairs.Add(otherEntity, new List<Entity>());
                        }
                        correctedEntityPairs[otherEntity].Add(entity);
                    }
                }
            }
        }

        private void SeparateEntities(Entity entityA, Entity entityB, float separationDistance)
        {
            TransformComponent transformA = entityA.GetComponent<TransformComponent>();
            MovementComponent movementA = entityA.GetComponent<MovementComponent>();

            TransformComponent transformB = entityB.GetComponent<TransformComponent>();
            MovementComponent movementB = entityB.GetComponent<MovementComponent>();

            // Seperation distance is based on their relative movement speed
            float aSpeed = movementA.MovementVector.Length();
            float bSpeed = movementB.MovementVector.Length();
            float combinedMovementSpeed = aSpeed + bSpeed;
            if(combinedMovementSpeed <= 0)
            {
                return;
            }
            float aContribution = aSpeed / combinedMovementSpeed;
            float bContribution = bSpeed / combinedMovementSpeed;

            // Total distance between A and B needs to be `separationDistance`
            float currentDistance = (transformA.Position - transformB.Position).Length();
            Vector2 center = (transformA.Position - transformB.Position) / 2 + transformB.Position;
            Vector2 centerToA = transformA.Position - center;
            centerToA.Normalize();
            Vector2 centerToB = transformB.Position - center;
            centerToB.Normalize();

            float distanceDifference = separationDistance - currentDistance;
            float aDistanceToMove = distanceDifference * aContribution;
            float bDistanceToMove = distanceDifference * bContribution;

            transformA.SetPosition(transformA.Position + centerToA * aDistanceToMove);
            transformB.SetPosition(transformB.Position + centerToB * bDistanceToMove);
        }

        private void InsertIntersectingChildNodes(Vector2 pos, float radius, QuadTreeNode ownerNode, QuadTreeNode node, ref List<QuadTreeNode> nodes)
        {
            if (!IntersectCircleRect(pos, radius, node.boundingRect))
            {
                // Stop here; if a higher node doesn't intersect, the child nodes are guarenteed not to intersect either
                return;
            }

            // Only add this node if there are entities to check against
            if(node.leaves.Count > 0)
            {
                nodes.Add(node);
            }

            if (node.subNodes.Count > 0)
            {
                foreach (QuadTreeNode subNode in node.subNodes)
                {
                    InsertIntersectingChildNodes(pos, radius, ownerNode, subNode, ref nodes);
                }
            }
        }

        private bool IntersectCircleRect(Vector2 circlePos, float circleRadius, BoundingRect rect)
        {
            return PointInRectangle(rect, circlePos)
                || IntersectCircleSegment(circlePos, circleRadius, new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top))
                || IntersectCircleSegment(circlePos, circleRadius, new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom))
                || IntersectCircleSegment(circlePos, circleRadius, new Vector2(rect.Right, rect.Bottom), new Vector2(rect.Left, rect.Bottom))
                || IntersectCircleSegment(circlePos, circleRadius, new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Left, rect.Top));
        }

        private bool PointInRectangle(BoundingRect rect, Vector2 point)
        {
            return rect.Contains(point);
        }

        private bool IntersectCircleSegment(Vector2 circlePos, float circleRadius, Vector2 a, Vector2 b)
        {
            float circleRadiusSquared = circleRadius * circleRadius;

            if((a - circlePos).LengthSquared() <= circleRadiusSquared)
            {
                return true;
            }
            if ((b - circlePos).LengthSquared() <= circleRadiusSquared)
            {
                return true;
            }

            Vector2 ab = b - a;
            ab.Normalize();
            Vector2 aCircle = circlePos - a;
            float lengthAlongAB = Vector2.Dot(ab, aCircle);
            Vector2 pointAlongAB = ab * lengthAlongAB + a;
            return (pointAlongAB - circlePos).LengthSquared() <= circleRadiusSquared;
        }

        protected override void OnUpdate(float dt)
        {
            Update(dt);
        }

        protected override void OnInitialize()
        {
            return;
        }

        protected override void OnKill()
        {
            return;
        }

        protected override void OnTogglePause()
        {
            return;
        }
    }
}
