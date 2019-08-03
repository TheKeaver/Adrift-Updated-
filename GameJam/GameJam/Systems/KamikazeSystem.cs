using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class KamikazeSystem : BaseSystem
    {
        Family _kamikazeFamily = Family.All(typeof(KamikazeComponent), typeof(EnemyComponent), typeof(MovementComponent), typeof(TransformComponent), typeof(CollisionComponent)).Get();
        ImmutableList<Entity> _kamikazeEntities;

        public KamikazeSystem(Engine engine) : base(engine)
        {
            _kamikazeEntities = engine.GetEntitiesFor(_kamikazeFamily);
        }

        public override void Update(float dt)
        {
            throw new NotImplementedException();
        }
    }
}
