using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class MenuBackgroundDestructionSystem : BaseSystem
    {
        private readonly Family _menuBackgroundFamily = Family.All(typeof(TransformComponent), typeof(MenuBackgroundComponent)).Get();

        private readonly ImmutableList<Entity> _menuBackgroundEntities;

        public MenuBackgroundDestructionSystem(Engine engine) : base(engine)
        {
            _menuBackgroundEntities = Engine.GetEntitiesFor(_menuBackgroundFamily);
        }

        public override void Update(float dt)
        {
            for(int i = 0; i < _menuBackgroundEntities.Count; i++)
            {
                TransformComponent transformComp = _menuBackgroundEntities[i].GetComponent<TransformComponent>();
                if(transformComp.Position.X < CVars.Get<float>("entity_background_spawner_destruction_x"))
                {
                    Engine.DestroyEntity(_menuBackgroundEntities[i--]);
                    continue;
                }
            }
        }
    }
}
