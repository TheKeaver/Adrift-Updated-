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
            MovementComponent moveComp = entity.GetComponent<MovementComponent>();
            TransformComponent transform = entity.GetComponent<TransformComponent>();

            if(moveComp.MovementVector.Length() >= 1)
            {
                DrawQuadOnlyEntity.Create(Engine, transform.Position, new Vector2[] {
                    // Current "Left/Top",
                    // Current "Right/Bottom",
                    // Last "Left/Top",
                    // Last "Right/Bottom",
                    // entity
                });
            }
        }
    }
}
