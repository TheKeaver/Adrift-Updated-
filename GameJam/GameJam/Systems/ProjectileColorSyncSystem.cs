using Audrey;
using GameJam.Components;
using System;

namespace GameJam.Systems
{
    class ProjectileColorSyncSystem : BaseSystem
    {
        // The reason this has .One() is because we only want to loop
        // through projectiles that need to be color synced. This system
        // will sync the color of ProjectileComponent into VectorSpriteComponent
        // and ColoredExplosionComponent, so if a projectile has neither of those
        // components we don't need to include it.
        readonly Family _projectileSyncFamily = Family.All(typeof(ProjectileComponent)).One(typeof(VectorSpriteComponent), typeof(ColoredExplosionComponent)).Get();

        readonly ImmutableList<Entity> _projectileSyncEntities;

        public ProjectileColorSyncSystem(Engine engine) : base(engine)
        {
            _projectileSyncEntities = Engine.GetEntitiesFor(_projectileSyncFamily);
        }

        public override void Update(float dt)
        {
            foreach(Entity entity in _projectileSyncEntities)
            {
                ProjectileComponent projectileComp = entity.GetComponent<ProjectileComponent>();
                Console.WriteLine(projectileComp.Color.ToString());

                if(entity.HasComponent<VectorSpriteComponent>())
                {
                    entity.GetComponent<VectorSpriteComponent>().ChangeColor(projectileComp.Color);
                }
                if(entity.HasComponent<ColoredExplosionComponent>())
                {
                    entity.GetComponent<ColoredExplosionComponent>().Color = projectileComp.Color;
                }
            }
        }
    }
}
