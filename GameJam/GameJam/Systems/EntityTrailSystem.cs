using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class EntityTrailSystem : BaseSystem
    {
        readonly Family _entityTrailFamily = Family.All(typeof(MovementComponent), typeof(EntityTrailComponent)).Get();
        readonly ImmutableList<Entity> _entityTrailEntities;

        public EntityTrailSystem(Engine engine) : base(engine)
        {
            _entityTrailEntities = engine.GetEntitiesFor(_entityTrailFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity trailEntity in _entityTrailEntities)
            {
                DrawPlayerTrail(trailEntity);
            }
        }

        private void DrawPlayerTrail(Entity ship)
        {

        }
    }
}
