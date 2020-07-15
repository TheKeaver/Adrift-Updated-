using Audrey;
using GameJam.Common;
using GameJam.Components;

namespace GameJam.Systems
{
    public class MenuBackgroundDestructionSystem : BaseSystem
    {
        private readonly Family _menuBackgroundFamily = Family.All(typeof(TransformComponent), typeof(MenuBackgroundComponent)).Get();

        private readonly ImmutableList<Entity> _menuBackgroundEntities;

        public Camera Camera
        {
            get;
            private set;
        }

        public MenuBackgroundDestructionSystem(Engine engine, Camera camera) : base(engine)
        {
            Camera = camera;

            _menuBackgroundEntities = Engine.GetEntitiesFor(_menuBackgroundFamily);
        }

        public void Update(float dt)
        {
            float zoom = Camera.TotalZoom;

            for(int i = 0; i < _menuBackgroundEntities.Count; i++)
            {
                TransformComponent transformComp = _menuBackgroundEntities[i].GetComponent<TransformComponent>();
                if(transformComp.Position.X < CVars.Get<float>("entity_background_spawner_destruction_x") / zoom)
                {
                    Engine.DestroyEntity(_menuBackgroundEntities[i--]);
                    continue;
                }
            }
        }

        protected override void OnUpdate(float dt)
        {
            Update(dt);
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
