using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

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
            float distanceBetween = (entity.GetComponent<TransformComponent>().Position - entity.GetComponent<EntityMirroringComponent>().entityToMirror.GetComponent<TransformComponent>().Position).Length();
            Console.WriteLine("Distance between Center of Ship and Center of VectorSpriteTrail: " + distanceBetween);

            MovementComponent moveComp = entity.GetComponent<VectorSpriteTrailComponent>().playerShip.GetComponent<MovementComponent>();
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            if(moveComp.MovementVector.Length() >= 200)
            {
                Vector2 lastTransform = thc.GetTransformHistoryAt(-2);
                float lastRotation = thc.GetRotationHistoryAt(-2);

                Vector2 point2 = FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation - (float)Math.PI);
                Vector2 point1 = FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation);
                Vector2 point3 = FindCalculatedPoint(transform.Position, lastTransform, lastRotation);
                Vector2 point4 = FindCalculatedPoint(transform.Position, lastTransform, lastRotation - (float)Math.PI);

                Console.WriteLine("Point 2: " + point2);
                Console.WriteLine("Point 1: " + point1);
                Console.WriteLine("Point 3: " + point3);
                Console.WriteLine("Point 4: " + point4);
                

                Entity quad = DrawQuadOnlyEntity.Create(Engine, transform.Position, new Vector2[] {
                    // No idea what the correct order is in practice, but should be
                    // clockwise
                    FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation - (float)Math.PI), // 2
                    FindCalculatedPoint(transform.Position, transform.Position, transform.Rotation), // 1
                    FindCalculatedPoint(transform.Position, lastTransform, lastRotation), // 3
                    FindCalculatedPoint(transform.Position, lastTransform, lastRotation - (float)Math.PI), // 4
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
    }
}
