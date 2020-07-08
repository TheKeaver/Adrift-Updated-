using Audrey;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

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
                Vector2 zero = thc.GetTransformHistoryAt(-1);
                Vector2 two = thc.GetTransformHistoryAt(-3);
                Vector2 three = thc.GetTransformHistoryAt(-4);
                Vector2 one = thc.GetTransformHistoryAt(-2);

                Entity quad = DrawQuadOnlyEntity.Create(Engine, transform.Position, new Vector2[] {
                    // No idea what the correct order is in practice, but should be
                    // clockwise
                    // Current "Right/Bottom",
                    // Last "Left/Top",
                    // Last "Right/Bottom",
                    // entity
                    thc.GetTransformHistoryAt(-1),
                    thc.GetTransformHistoryAt(-3),
                    thc.GetTransformHistoryAt(-4),
                    thc.GetTransformHistoryAt(-2)
                }, entity.GetComponent<VectorSpriteTrailComponent>().playerShip);

                Console.WriteLine("Zero: " + zero);
                Console.WriteLine("Two: " + two);
                Console.WriteLine("Three: " + three);
                Console.WriteLine("One: " + one);

                quad.AddComponent(new FadingEntityTimerComponent(CVars.Get<float>("animation_trail_fading_timer")));
            }
        }
    }
}
