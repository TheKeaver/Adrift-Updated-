using System;
using Audrey;
using GameJam.Components;

namespace GameJam.Processes.Enemies
{
    public class LaserEnemyUnfreezeRotation : InstantProcess
    {
        private readonly Engine Engine;
        private Entity LaserEnemyEntity;

        public LaserEnemyUnfreezeRotation(Engine engine, Entity laserEnemyEntity)
        {
            Engine = engine;
            LaserEnemyEntity = laserEnemyEntity;
        }

        protected override void OnTrigger()
        {
            if (!Engine.GetEntities().Contains(LaserEnemyEntity))
            {
                return;
            }

            LaserEnemyEntity.GetComponent<RotationComponent>().RotationSpeed = CVars.Get<float>("laser_enemy_rotational_speed");
        }
    }
}
