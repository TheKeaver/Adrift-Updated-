using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class TransformResetSemiSystem
    {
        public Engine Engine
        {
            get;
            private set;
        }

        public bool ReadyForReset
        {
            get;
            private set;
        }

        private readonly Family _transformFamily = Family.All(typeof(TransformComponent)).Get();

        private readonly ImmutableList<Entity> _transformEntities;

        public TransformResetSemiSystem(Engine engine)
        {
            Engine = engine;

            ReadyForReset = true;

            _transformEntities = engine.GetEntitiesFor(_transformFamily);
        }

        public void OnUpdate()
        {
            if (ReadyForReset)
            {
                ReadyForReset = false;

                foreach (Entity entity in _transformEntities)
                {
                    TransformComponent transformComp = entity.GetComponent<TransformComponent>();

                    transformComp.ResetAll();
                }
            }
        }

        public void OnRender()
        {
            ReadyForReset = true;
        }
    }
}
