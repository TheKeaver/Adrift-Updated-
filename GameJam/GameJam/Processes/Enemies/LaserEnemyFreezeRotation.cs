using Audrey;
using GameJam.Components;

namespace GameJam.Processes.Enemies
{
    public class LaserEnemyFreezeRotation : InstantProcess
    {
        private readonly Engine Engine;
        private Entity LaserEnemyEntity;

        public LaserEnemyFreezeRotation(Engine engine, Entity laserEnemyEntity)
        {
            Engine = engine;
            LaserEnemyEntity = laserEnemyEntity;
        }

        protected override void OnTrigger()
        {
            if(!Engine.GetEntities().Contains(LaserEnemyEntity))
            {
                return;
            }

            LaserEnemyEntity.GetComponent<RotationComponent>().RotationSpeed = 0;
        }
    }
}
