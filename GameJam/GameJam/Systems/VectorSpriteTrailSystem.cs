using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Systems
{
    public class VectorSpriteTrailSystem : BaseSystem
    {
        readonly Family _vectorSpriteTrailFamily = Family.All(typeof(VectorSpriteTrailComponent), typeof(TransformHistoryComponent)).Get();
        readonly ImmutableList<Entity> _vectorSpriteTrailEntities;

        public VectorSpriteTrailSystem(Engine engine) : base(engine)
        {
            _vectorSpriteTrailEntities = engine.GetEntitiesFor(_vectorSpriteTrailFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity trailEntity in _vectorSpriteTrailEntities)
            {
                TransformHistoryComponent thc = trailEntity.GetComponent<TransformHistoryComponent>();

                thc.updateInterval.Update(dt);
                if(thc.updateInterval.HasElapsed())
                {
                    thc.updateInterval.Reset();
                    DrawVectorSpriteTrail(trailEntity, thc);
                }
            }
        }

        private void DrawVectorSpriteTrail(Entity entity, TransformHistoryComponent thc)
        {
            MovementComponent moveComp = entity.GetComponent<VectorSpriteTrailComponent>().playerShip.GetComponent<MovementComponent>();
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            if(moveComp.MovementVector.Length() >= 1)
            {
                Vector2 lastTransform = thc.GetTransformHistoryAt(-2);
                float lastRotation = thc.GetRotationHistoryAt(-2);

                
                Vector2 point2 = FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation - (float)Math.PI);
                Vector2 point1 = FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation);
                Vector2 point3 = FindCalculatedPoint(transform.Position, lastTransform, lastRotation);
                Vector2 point4 = FindCalculatedPoint(transform.Position, lastTransform, lastRotation - (float)Math.PI);

                Console.Write(point2);
                Console.Write(point1);
                Console.Write(point3);
                Console.Write(point4);
                

                Entity quad = DrawQuadOnlyEntity.Create(Engine, transform.Position, new Vector2[] {
                    // No idea what the correct order is in practice, but should be
                    // clockwise
                    FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation - (float)Math.PI), // 2
                    FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation), // 1
                    FindCalculatedPoint(transform.Position, lastTransform, lastRotation), // 3
                    FindCalculatedPoint(transform.Position, lastTransform, lastRotation - (float)Math.PI), // 4
                }, entity.GetComponent<VectorSpriteTrailComponent>().playerShip);

                Console.WriteLine("-1: " + transform.Position);
                Console.WriteLine("-2: " + lastTransform);

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
            float opp = (CVars.Get<float>("animation_trail_width")/2) * (float)(Math.Sin(rotation) + (Math.PI/2));
            float adj = (CVars.Get<float>("animation_trail_width")/2) * (float)(Math.Cos(rotation) + (Math.PI/2));

            Vector2 ret = currentTransform - locationTransform;

            ret.X += adj;
            ret.Y += opp;

            return ret;
        }
    }
}
