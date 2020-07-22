using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Systems
{
    public class VectorSpriteTrailSystem : BaseSystem
    {
        readonly Family _vectorSpriteTrailFamily = Family.All(typeof(VectorSpriteTrailComponent), typeof(TransformHistoryComponent), typeof(MovementComponent)).Get();
        readonly ImmutableList<Entity> _vectorSpriteTrailEntities;

        public VectorSpriteTrailSystem(Engine engine) : base(engine)
        {
            _vectorSpriteTrailEntities = engine.GetEntitiesFor(_vectorSpriteTrailFamily);
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

        protected override void OnUpdate(float dt)
        {
            foreach (Entity trailEntity in _vectorSpriteTrailEntities)
            {
                TransformHistoryComponent thc = trailEntity.GetComponent<TransformHistoryComponent>();

                thc.updateInterval.Update(dt);
                if (thc.updateInterval.HasElapsed())
                {
                    thc.updateInterval.Reset();
                    DrawVectorSpriteTrail(trailEntity, thc);
                }
            }
        }

        private void DrawVectorSpriteTrail(Entity entity, TransformHistoryComponent thc)
        {
            float distanceBetween = (entity.GetComponent<TransformComponent>().Position - entity.GetComponent<EntityMirroringComponent>().entityToMirror.GetComponent<TransformComponent>().Position).Length();
            Console.WriteLine("Distance between Center of Ship and Center of VectorSpriteTrail: " + distanceBetween);

            MovementComponent moveComp = entity.GetComponent<VectorSpriteTrailComponent>().playerShip.GetComponent<MovementComponent>();
            TransformComponent transform = entity.GetComponent<TransformComponent>();
            Vector2 direction = new Vector2((float)Math.Cos(transform.Rotation), (float)Math.Sin(transform.Rotation));

            if(moveComp.MovementVector.Length() >= CVars.Get<float>("player_trail_minimum_movement"))
            {
                Vector2 lastTransform = thc.Positions[1];// thc.GetTransformHistoryAt(-2);
                float lastRotation = thc.Rotations[1]; // thc.GetRotationHistoryAt(-2);
                Vector2 lastDirection = new Vector2((float)Math.Cos(lastRotation), (float)Math.Sin(lastRotation));

                Vector2 point1, point2;
                CalculateEndsAtLocation(transform.Position, transform.Position, direction,
                    out point1, out point2);
                Vector2 point3, point4;
                CalculateEndsAtLocation(transform.Position, lastTransform, lastDirection,
                    out point3, out point4);

                Vector2[] points = new Vector2[4];
                if(Vector2.Dot(direction, lastDirection) > 1)
                {
                    points[0] = point1;
                    points[1] = point2;
                    points[2] = point4;
                    points[3] = point3;
                } else
                {
                    points[0] = point2;
                    points[1] = point1;
                    points[2] = point3;
                    points[3] = point4;
                }

                Entity quad = DrawQuadOnlyEntity.Create(Engine, transform.Position, new Vector2[] {
                    // No idea what the correct order is in practice, but should be
                    // clockwise
                    point1,
                    point2,
                    point4,
                    point3
                }, entity.GetComponent<VectorSpriteTrailComponent>().playerShip);

                Console.WriteLine("Current: " + transform.Position);
                Console.WriteLine("Previous: " + lastTransform);

                quad.AddComponent(new FadingEntityTimerComponent(CVars.Get<float>("animation_trail_fading_timer")));
            }
        }

        /* 
         * Find the point needed based off of:
         *  - Current Transform of VectorSpriteTrailEntity
         *  - Center Transform of the two points being drawn
         *  - Rotation of the ship at the time of drawing
         */
        private Vector2 FindCalculatedPoint(Vector2 currentTransform, Vector2 locationTransform, float rotation)
        {
            float translatedRotation = (float)(Math.PI / 2) + rotation;

            float opp = (CVars.Get<float>("animation_trail_width") / 2) * (float)Math.Sin(translatedRotation);
            float adj = (CVars.Get<float>("animation_trail_width") / 2) * (float)Math.Cos(translatedRotation);

            Vector2 ret = currentTransform - locationTransform;

            ret.X += adj;
            ret.Y += opp;

            return ret;
        }
        private void CalculateEndsAtLocation(Vector2 origin, Vector2 location, Vector2 direction,
            out Vector2 p1, out Vector2 p2)
        {
            Vector2 normal = new Vector2(-direction.Y, direction.X);

            Vector2 lp1 = normal * CVars.Get<float>("animation_trail_width") / 2 + location;
            Vector2 lp2 = -normal * CVars.Get<float>("animation_trail_width") / 2 + location;

            p1 = lp1 - origin;
            p2 = lp2 - origin;
        }
    }
}
