using System;
using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class LaserEnemySystem : BaseSystem
    {
        private readonly Family _laserEnemyFamily = Family.All(typeof(LaserEnemyComponent), typeof(TransformComponent)).Get();
        private readonly Family _raycastFamily = Family.All(typeof(TransformComponent), typeof(EdgeComponent), typeof(CollisionComponent)).Get();
        private readonly Family _raycastWithShieldFamily = Family.All(typeof(TransformComponent), typeof(CollisionComponent)).One(typeof(EdgeComponent), typeof(PlayerShieldComponent)).Get();

        private ImmutableList<Entity> _laserEnemyEntities;
        private ImmutableList<Entity> _raycastEntities;
        private ImmutableList<Entity> _raycastWithShieldEntities;

        public LaserEnemySystem(Engine engine) : base(engine)
        {
            _laserEnemyEntities = Engine.GetEntitiesFor(_laserEnemyFamily);
            _raycastEntities = Engine.GetEntitiesFor(_raycastFamily);
            _raycastWithShieldEntities = Engine.GetEntitiesFor(_raycastWithShieldFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity laserEnemyEntity in _laserEnemyEntities)
            {
                LaserEnemyComponent laserEnemyComp = laserEnemyEntity.GetComponent<LaserEnemyComponent>();
                Entity laserBeamEntity = laserEnemyComp.LaserBeamEntity;

                if(laserBeamEntity != null)
                {
                    LaserBeamComponent laserBeamComp = laserBeamEntity.GetComponent<LaserBeamComponent>();
                    if(laserBeamComp == null)
                    {
                        throw new Exception("Laser beam does not have a `LaserBeamComponent`.");
                    }

                    TransformComponent transformComp = laserEnemyEntity.GetComponent<TransformComponent>();
                    float cos = (float)Math.Cos(transformComp.Rotation),
                        sin = (float)Math.Sin(transformComp.Rotation);
                    Vector2 laserEnemyTip = new Vector2(4 + 0.35f / 1.5f, 0); // Right-most extent of laser enemy
                    laserEnemyTip = new Vector2(cos * laserEnemyTip.X - sin * laserEnemyTip.Y,
                        sin * laserEnemyTip.X + cos * laserEnemyTip.Y);
                    laserEnemyTip *= transformComp.Scale;
                    laserEnemyTip += transformComp.Position;

                    Vector2 laserEnemyDirection = new Vector2((float)Math.Cos(transformComp.Rotation), (float)Math.Sin(transformComp.Rotation)); // laserEnemyTip - transformComp.Position;
                    laserEnemyDirection.Normalize();

                    // Simple raycast to find edge/shield this laser touches
                    RaycastHit laserHit = Raycast(laserBeamComp.InteractWithShield ? _raycastWithShieldEntities : _raycastEntities, laserEnemyTip, laserEnemyDirection);

                    if (laserHit.Other.HasComponent<PlayerShieldComponent>() /*&& laserHit.Other.GetComponent<PlayerShieldComponent>().LaserReflectionActive == true*/ && laserBeamComp.ComputeReflection)
                    {
                        Vector2 shieldNormal = laserHit.Position - laserHit.Other.GetComponent<PlayerShieldComponent>().ShipEntity.GetComponent<TransformComponent>().Position;
                        if (Vector2.Dot(shieldNormal, laserHit.Normal) > 0)
                        {
                            if (laserBeamComp.ReflectionBeamEntity == null)
                            {
                                laserBeamComp.ReflectionBeamEntity = LaserBeamEntity.Create(Engine, Vector2.Zero, laserBeamEntity.HasComponent<CollisionComponent>());
                                laserBeamComp.ReflectionBeamEntity.AddComponent(new LaserBeamReflectionComponent());
                                if (laserHit.Other.HasComponent<PlayerComponent>()) {
                                    laserBeamComp.ReflectionBeamEntity.GetComponent<LaserBeamReflectionComponent>().ReflectedBy
                                        = laserHit.Other.GetComponent<PlayerComponent>().Player;
                                }
                            }

                            Entity reflectionBeamEntity = laserBeamComp.ReflectionBeamEntity;
                            reflectionBeamEntity.GetComponent<LaserBeamComponent>().Thickness = laserBeamComp.Thickness;
                            Vector2 laserDirection = laserHit.Position - laserEnemyTip;
                            Vector2 beamOutDirection = GetReflectionVector(laserDirection, laserHit.Normal);
                            // Simple raycast to find edge this laser touches
                            RaycastHit reflectionHit = Raycast(_raycastEntities, laserHit.Position, beamOutDirection);
                            SetLaserBeamProperties(reflectionBeamEntity,
                                laserHit.Position,
                                reflectionHit.Position,
                                (float)Math.Atan2(beamOutDirection.Y, beamOutDirection.X),
                                reflectionBeamEntity.GetComponent<LaserBeamComponent>().Thickness);
                        }
                    }
                    else
                    {
                        if (laserBeamComp.ReflectionBeamEntity != null)
                        {
                            Engine.DestroyEntity(laserBeamComp.ReflectionBeamEntity);
                            laserBeamComp.ReflectionBeamEntity = null;
                        }
                    }

                    SetLaserBeamProperties(laserBeamEntity, laserEnemyTip, laserHit.Position, transformComp.Rotation, laserBeamComp.Thickness);
                }
            }
        }

        private void SetLaserBeamProperties(Entity laserBeamEntity, Vector2 laserBeamStart, Vector2 laserBeamEnd, float rotation, float thickness)
        {
            LaserBeamComponent laserBeamComp = laserBeamEntity.GetComponent<LaserBeamComponent>();

            double laserBeamLength = (laserBeamEnd - laserBeamStart).Length();

            Vector2 lb1 = new Vector2((float)laserBeamLength, -thickness / 2);
            Vector2 lb2 = new Vector2((float)laserBeamLength, thickness / 2);
            Vector2 lb3 = new Vector2(0, thickness / 2);
            Vector2 lb4 = new Vector2(0, -thickness / 2);
            laserBeamEntity.GetComponent<VectorSpriteComponent>().RenderShapes[0] = new QuadRenderShape(
                lb4, lb3, lb2, lb1,
                laserBeamComp.Color);

            CollisionComponent collisionComp = laserBeamEntity.GetComponent<CollisionComponent>();
            if(collisionComp != null)
            {
                ((PolygonCollisionShape)collisionComp.CollisionShapes[0]).Vertices = new Vector2[]
                {
                    lb1, lb2, lb3, lb4
                };
            }

            TransformComponent laserBeamTransformComp = laserBeamEntity.GetComponent<TransformComponent>();
            laserBeamTransformComp.Move(laserBeamStart - laserBeamTransformComp.Position);
            laserBeamTransformComp.Rotate(rotation - laserBeamTransformComp.Rotation);
        }

        struct RaycastHit
        {
            public Vector2 Position;
            public Vector2 Normal;
            public double LengthSquared;
            public Entity Other;
        }

        private RaycastHit Raycast(ImmutableList<Entity> raycastEntities, Vector2 origin, Vector2 direction)
        {
            Vector2 v1 = new Vector2(-direction.Y, direction.X);

            RaycastHit hit = new RaycastHit
            {
                LengthSquared = double.PositiveInfinity
            };
            // Only raycasts against polygons (not circles)
            foreach (Entity raycastEntity in raycastEntities)
            {
                bool performRayCast = true;

                TransformComponent raycastTransformComp = raycastEntity.GetComponent<TransformComponent>();
                CollisionComponent raycastCollisionComp = raycastEntity.GetComponent<CollisionComponent>();

                if((raycastCollisionComp.CollisionMask & Constants.Collision.COLLISION_GROUP_RAYCAST) == 0)
                {
                    performRayCast = false;
                }

                // Make or find a raycast collision group

                if (performRayCast)
                {
                    float cos = (float)Math.Cos(raycastTransformComp.Rotation),
                        sin = (float)Math.Sin(raycastTransformComp.Rotation);

                    foreach (CollisionShape collisionShape in raycastCollisionComp.CollisionShapes)
                    {
                        PolygonCollisionShape polygonShape = collisionShape as PolygonCollisionShape;
                        if (polygonShape != null)
                        {
                            for (int i = 0; i < polygonShape.Vertices.Length; i++)
                            {
                                int j = (i + 1) % polygonShape.Vertices.Length;

                                Vector2 a = polygonShape.Vertices[i] + polygonShape.Offset;
                                a *= raycastTransformComp.Scale;
                                a = new Vector2(cos * a.X - sin * a.Y, sin * a.X + cos * a.Y);
                                a += raycastTransformComp.Position;

                                Vector2 b = polygonShape.Vertices[j] + polygonShape.Offset;
                                b *= raycastTransformComp.Scale;
                                b = new Vector2(cos * b.X - sin * b.Y, sin * b.X + cos * b.Y);
                                b += raycastTransformComp.Position;

                                Vector2 v2 = a - b;
                                Vector2 v3 = a - origin;
                                Vector2 v4 = new Vector2(a.Y - b.Y, b.X - a.X);

                                double det = 1 / Vector2.Dot(v1, v2);
                                double t1 = det * Vector2.Dot(v3, v4);
                                double t2 = det * Vector2.Dot(v1, v3);

                                if (t2 >= 0 && t2 <= 1
                                    && t1 >= 0)
                                {
                                    // Hit
                                    Vector2 newHit = (b - a) * (float)t2 + a;
                                    float newLengthSquared = (newHit - origin).LengthSquared();
                                    if (newLengthSquared < hit.LengthSquared)
                                    {
                                        hit.Position = newHit;
                                        hit.Normal = new Vector2(-v2.Y, v2.X);
                                        if (Vector2.Dot(direction, hit.Normal) > 0)
                                        {
                                            hit.Normal *= -1;
                                        }
                                        hit.Normal.Normalize();
                                        hit.LengthSquared = newLengthSquared;
                                        hit.Other = raycastEntity;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return hit;
        }

        Vector2 GetReflectionVector(Vector2 colliding, Vector2 normal)
        {
            return colliding - 2 * Vector2.Dot(colliding, normal) * normal;
        }
    }
}
