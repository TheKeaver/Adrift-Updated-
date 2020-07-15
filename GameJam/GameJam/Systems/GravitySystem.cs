using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class GravitySystem : BaseSystem
    {
        readonly Family _entitiesAffectedFamily = Family.All(typeof(MovementComponent), typeof(TransformComponent))
                                                        .One(typeof(PlayerShipComponent), typeof(ProjectileComponent), typeof(EnemyComponent))
                                                        .Exclude(typeof(LaserEnemyComponent), typeof(ShootingEnemyComponent)).Get();
        readonly ImmutableList<Entity> _affectedEntities;

        readonly Family _gravityFamily = Family.All(typeof(TransformComponent), typeof(GravityHoleEnemyComponent)).Get();
        readonly ImmutableList<Entity> _gravityEntities;

        public GravitySystem(Engine engine) : base(engine)
        {
            _affectedEntities = engine.GetEntitiesFor(_entitiesAffectedFamily);
            _gravityEntities = engine.GetEntitiesFor(_gravityFamily);
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
            foreach (Entity affectedEntity in _affectedEntities)
            {
                ProcessGravity(affectedEntity, dt);
            }
        }

        void ProcessGravity(Entity affectedEntity, float dt)
        {
            TransformComponent transformComp = affectedEntity.GetComponent<TransformComponent>();
            MovementComponent movementComp = affectedEntity.GetComponent<MovementComponent>();

            foreach (Entity gravityEntity in _gravityEntities)
            {
                if( (transformComp.Position - gravityEntity.GetComponent<TransformComponent>().Position).Length() <=
                        CVars.Get<float>("gravity_hole_enemy_radius"))
                {
                    Vector2 gravityForceDirection = gravityEntity.GetComponent<TransformComponent>().Position - transformComp.Position;
                    gravityForceDirection.Normalize();
                    gravityForceDirection *= CVars.Get<float>("gravity_hole_enemy_force") * dt;

                    affectedEntity.GetComponent<MovementComponent>().MovementVector += gravityForceDirection;
                }
            }
        }
    }
}
