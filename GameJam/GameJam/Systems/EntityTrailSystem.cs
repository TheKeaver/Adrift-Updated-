using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Reflection;

namespace GameJam.Systems
{
    public class EntityTrailSystem : BaseSystem
    {
        readonly Family _entityTrailFamily = Family.All(typeof(MovementComponent), typeof(FadingEntityComponent)).Get();
        readonly ImmutableList<Entity> _entityTrailEntities;

        public EntityTrailSystem(Engine engine) : base(engine)
        {
            _entityTrailEntities = engine.GetEntitiesFor(_entityTrailFamily);
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
            foreach (Entity trailEntity in _entityTrailEntities)
            {
                TransformHistoryComponent transformHistory = trailEntity.GetComponent<TransformHistoryComponent>();

                transformHistory.updateInterval.Update(dt);
                if (transformHistory.updateInterval.HasElapsed())
                {
                    transformHistory.updateInterval.Reset();
                    DrawEntityTrail(trailEntity);
                }
            }
        }

        private void DrawEntityTrail(Entity entity)
        {
            MovementComponent moveComp = entity.GetComponent<MovementComponent>();
            TransformHistoryComponent transformHistory = entity.GetComponent<TransformHistoryComponent>();
            FadingEntityComponent fec = entity.GetComponent<FadingEntityComponent>();
            Entity spriteOnly;

            if(moveComp.MovementVector.Length() >= 1)
            {
                // TODO: EntityTrailSystem does not draw trails that match Player Color, instead each is just drawn as White
                // Entity Create Sprite only for player should accept a color into the argument
                // This, however, would make its syntax differ from other CreateSpriteOnly methods and would need to be made to match
                MethodInfo me = fec.thisType.GetMethod("CreateSpriteOnly", new Type[] { typeof(Engine), typeof(Vector2), typeof(float) });
                int lastHistory = transformHistory.GetLastHistoryIndex();

                spriteOnly = (Entity)me.Invoke(null, new object[]
                {
                    Engine,
                    transformHistory.positionHistory[lastHistory],
                    transformHistory.rotationHistory[lastHistory]
                });

                spriteOnly.AddComponent(new FadingEntityTimerComponent(CVars.Get<float>("animation_trail_fading_timer")));
            }
        }
    }
}
