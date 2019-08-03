using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    /// <summary>
    /// A system that updates input snapshots each frame.
    /// </summary>
    public class InputSystem : BaseSystem
    {
        Family _family = Family.All(typeof(PlayerComponent)).Get();
        ImmutableList<Entity> _entities;

        public InputSystem(Engine engine) : base(engine)
        {
            _entities = Engine.GetEntitiesFor(_family);
        }

        public bool IsPauseButtonPressed()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                Entity entity = _entities[i];

                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                if (playerComp.Player.InputMethod.PauseKeyPressed)
                {
                    return true;
                }
            }
            return false;
        }

        public override void Update(float dt)
        {
            foreach (Entity entity in _entities)
            {
                PlayerComponent playerComp = entity.GetComponent<PlayerComponent>();
                playerComp.Player.InputMethod.Update(dt);
            }
        }
    }
}
