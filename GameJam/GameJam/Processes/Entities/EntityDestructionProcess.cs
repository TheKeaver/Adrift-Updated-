using Audrey;

namespace GameJam.Processes.Entities
{
    public class EntityDestructionProcess : InstantProcess
    {
        private Engine Engine;
        private Entity DestroyMe;
        public EntityDestructionProcess(Engine engine, Entity destroyMe)
        {
            DestroyMe = destroyMe;
            Engine = engine;
        }
        protected override void OnTrigger()
        {
            if (Engine.GetEntities().Contains(DestroyMe))
            {
                Engine.DestroyEntity(DestroyMe);
            }
        }
    }
}
