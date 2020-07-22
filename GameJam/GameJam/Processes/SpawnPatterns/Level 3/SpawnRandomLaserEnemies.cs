using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomLaserEnemies : InstantProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnRandomLaserEnemies(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            for (int i = 0; i < simulatedCenters.Length; i++)
            {
                float angle = SPM.AngleFacingNearestPlayerShip(simulatedCenters[i]);
                LaserEnemyEntity.Spawn(Engine, ProcessManager, simulatedCenters[i], angle);
            }
        }

        public float GetMaxSpawnTimer()
        {
            return (CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") + CVars.Get<float>("animation_spawn_warp_phase_2_base_duration"))
                    * CVars.Get<float>("animation_spawn_warp_time_scale");
        }

        public int GetNumberOfValidCenters()
        {
            return 3;
        }

        public float GetMinimumValidRadius()
        {
            return 50;
        }
    }
}
