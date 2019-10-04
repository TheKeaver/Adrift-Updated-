using System;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class LaserEnemySystem : BaseSystem
    {
        private readonly Family _laserEnemyFamily = Family.All(typeof(LaserEnemyComponent), typeof(TransformComponent)).Get();
        private readonly Family _raycastFamily = Family.All(typeof(TransformComponent)).One(typeof(EdgeComponent), typeof(PlayerShieldComponent)).Get();

        private ImmutableList<Entity> _laserEnemyEntities;
        private ImmutableList<Entity> _raycastEntities;

        public LaserEnemySystem(Engine engine) : base(engine)
        {
            _laserEnemyEntities = Engine.GetEntitiesFor(_laserEnemyFamily);
            _raycastEntities = Engine.GetEntitiesFor(_raycastFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity laserEnemyEntity in _laserEnemyEntities)
            {
                LaserEnemyComponent laserEnemyComp = laserEnemyEntity.GetComponent<LaserEnemyComponent>();
                Entity laserBeamEntity = laserEnemyComp.LaserBeamEntity;

                if(laserBeamEntity != null)
                {
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
                    RaycastHit laserHit = Raycast(laserEnemyTip, laserEnemyDirection);

                    // TODO: Shield reflection

                    Vector2 laserBeamStart = laserEnemyTip;
                    Vector2 laserBeamEnd = laserHit.Position;

                    double laserBeamLength = (laserBeamEnd - laserBeamStart).Length();
                    float laserBeamThickness = 5;

                    // TODO: CollisionComponent
                    Vector2 lb1 = new Vector2((float)laserBeamLength, -laserBeamThickness / 2);
                    Vector2 lb2 = new Vector2((float)laserBeamLength, laserBeamThickness / 2);
                    Vector2 lb3 = new Vector2(0, laserBeamThickness / 2);
                    Vector2 lb4 = new Vector2(0, -laserBeamThickness / 2);
                    laserBeamEntity.GetComponent<VectorSpriteComponent>().RenderShapes[0] = new QuadRenderShape(
                        lb1, lb2, lb3, lb4,
                        Color.Red);

                    TransformComponent laserBeamTransformComp = laserBeamEntity.GetComponent<TransformComponent>();
                    laserBeamTransformComp.Move(laserBeamStart - laserBeamTransformComp.Position);
                    laserBeamTransformComp.Rotate(transformComp.Rotation - laserBeamTransformComp.Rotation);
                }
            }
        }

        struct RaycastHit
        {
            public Vector2 Position;
            public double LengthSquared;
            public Entity Other;
        }

        private RaycastHit Raycast(Vector2 origin, Vector2 direction)
        {
            Vector2 v1 = new Vector2(-direction.Y, direction.X);

            RaycastHit hit = new RaycastHit
            {
                LengthSquared = double.PositiveInfinity
            };
            foreach (Entity raycastEntity in _raycastEntities)
            {
                TransformComponent raycastTransformComp = raycastEntity.GetComponent<TransformComponent>();
                CollisionComponent raycastCollisionComp = raycastEntity.GetComponent<CollisionComponent>();

                float cos = (float)Math.Cos(raycastTransformComp.Rotation),
                    sin = (float)Math.Sin(raycastTransformComp.Rotation);

                foreach (CollisionShape collisionShape in raycastCollisionComp.CollisionShapes)
                {
                    PolygonCollisionShape polygonShape = collisionShape as PolygonCollisionShape;
                    if(polygonShape != null)
                    {
                        for(int i = 0; i < polygonShape.Vertices.Length; i++)
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

                            if(t2 >= 0 && t2 <= 1
                                && t1 >= 0)
                            {
                                // Hit
                                Vector2 newHit = (b - a) * (float)t2 + a;
                                if ((newHit - origin).LengthSquared() < hit.LengthSquared)
                                {
                                    hit.Position = newHit;
                                    hit.LengthSquared = newHit.LengthSquared();
                                    hit.Other = raycastEntity;
                                }
                            }
                        }
                    }
                }
            }

            return hit;
        }
    }
}
