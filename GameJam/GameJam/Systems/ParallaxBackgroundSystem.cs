using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;

namespace GameJam.Systems
{
    public class ParallaxBackgroundSystem : BaseSystem
    {
        readonly Family _parallaxFamily = Family.All(typeof(TransformComponent), typeof(ParallaxBackgroundComponent)).Get();

        readonly ImmutableList<Entity> _parallaxEntities;

        public Camera Camera
        {
            get;
            private set;
        }

        public ParallaxBackgroundSystem(Engine engine, Camera camera) : base(engine)
        {
            Camera = camera;

            _parallaxEntities = Engine.GetEntitiesFor(_parallaxFamily);
        }

        protected override void OnUpdate(float dt)
        {
            foreach (Entity parallaxEntity in _parallaxEntities)
            {
                ParallaxBackgroundComponent parallaxComp = parallaxEntity.GetComponent<ParallaxBackgroundComponent>();
                TransformComponent transformComp = parallaxEntity.GetComponent<TransformComponent>();

                Vector2 newPosition = Vector2.Lerp(parallaxComp.Origin,
                    Camera.Position,
                    parallaxComp.Strength);
                transformComp.Move(newPosition - transformComp.Position);
            }
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
    }
}
