using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class PassiveRotationSystem : BaseSystem
    {
        private readonly Family _rotationFamily = Family.All(typeof(RotationComponent), typeof(TransformComponent)).Exclude(typeof(EnemyComponent), typeof(PlayerShieldComponent), typeof(PlayerShipComponent), typeof(LaserBeamComponent), typeof(ProjectileComponent)).Get();

        private readonly ImmutableList<Entity> _rotationEntities;

        public PassiveRotationSystem(Engine engine) : base(engine)
        {
            _rotationEntities = engine.GetEntitiesFor(_rotationFamily);
        }

        public override void Update(float dt)
        {
            foreach(Entity rotationEntity in _rotationEntities)
            {
                RotationComponent rotationComp = rotationEntity.GetComponent<RotationComponent>();
                rotationEntity.GetComponent<TransformComponent>().Rotate(rotationComp.RotationSpeed * dt);
            }
        }
    }
}
