using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    /// <summary>
    /// A system that updates input snapshots each frame.
    /// </summary>
    public class InputSystem : BaseSystem
    {
        readonly Family _family = Family.All(typeof(PlayerComponent)).Get();
        readonly ImmutableList<Entity> _entities;

        public InputSystem(Engine engine) : base(engine)
        {
            _entities = Engine.GetEntitiesFor(_family);
        }

        protected override void OnUpdate(float dt)
        {
            foreach (Entity entity in _entities)
            {
                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                playerComp.Player.InputMethod.Update(dt);
            }
        }
    }
}
