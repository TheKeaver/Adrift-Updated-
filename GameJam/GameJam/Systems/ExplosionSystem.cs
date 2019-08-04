using System;
using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class ExplosionSystem : BaseSystem
    {
        readonly Family _explosionFamily = Family.All(typeof(ExplosionComponent), typeof(AnimationComponent)).Get();
        readonly ImmutableList<Entity> _explosionEntities;

        public ExplosionSystem(Engine engine) : base(engine)
        {
            _explosionEntities = Engine.GetEntitiesFor(_explosionFamily);
        }

        public override void Update(float dt)
        {
            for (int i = 0; i < _explosionEntities.Count; i++)
            {
                Entity explosionEntity = _explosionEntities[i];

                AnimationComponent animationComp = explosionEntity.GetComponent<AnimationComponent>();
                if (animationComp.ActiveAnimationIndex < 0)
                {
                    Engine.DestroyEntity(explosionEntity);
                    i--;
                }
            }
        }
    }
}
