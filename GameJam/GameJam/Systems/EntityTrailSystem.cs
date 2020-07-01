using Audrey;
using GameJam.Components;
using System.Reflection;

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

        private void DrawPlayerTrail(Entity entity)
        {
            MovementComponent moveComp = entity.GetComponent<MovementComponent>();
            TransformHistoryComponent transformHistory = entity.GetComponent<TransformHistoryComponent>();

            if(moveComp.MovementVector.Length() >= 20)
            {
               int i = ++transformHistory.currentTrailCounter;
               while (i > 0)
               {
                    // NEED "CreateSpriteOnly" FOR THE PLAYER SHIP ENTITY
                    MethodInfo me = entity.GetType().GetMethod("CreateSpriteOnly");

                    me.Invoke(this, new object[]
                        {
                            Engine,
                            transformHistory.positionHistory[i],
                            transformHistory.rotationHistory[i]
                        });
               }
            }
        }
    }
}
