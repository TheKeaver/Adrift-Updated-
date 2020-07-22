using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    class TransformResetSystem : BaseSystem
    {
        private readonly Family _transformFamily = Family.All(typeof(TransformComponent)).Get();

        private readonly ImmutableList<Entity> _transformEntities;

        public TransformResetSystem(Engine engine) : base(engine)
        {
            _transformEntities = engine.GetEntitiesFor(_transformFamily);
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
            foreach (Entity entity in _transformEntities)
            {
                TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                transformComp.ResetAll();
            }
        }
    }
}
