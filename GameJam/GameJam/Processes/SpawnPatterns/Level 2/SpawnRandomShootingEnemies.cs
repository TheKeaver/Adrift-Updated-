using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomShootingEnemies : InstantProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public int Count
        {
            get;
            private set;
        }

        public SpawnRandomShootingEnemies(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
            : this(engine, processManager, spm, 3)
        {
        }
        public SpawnRandomShootingEnemies(Engine engine, ProcessManager processManager, SpawnPatternManager spm, int count)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;

            Count = count;
        }

        protected override void OnTrigger()
        {
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            for (int i = 0; i < simulatedCenters.Length; i++)
            {
                float angle = SPM.AngleFacingNearestPlayerShip(simulatedCenters[i]);
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, simulatedCenters[i], angle);
            }
        }

        public float GetMaxSpawnTimer()
        {
            return (CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") + CVars.Get<float>("animation_spawn_warp_phase_2_base_duration"))
                   * CVars.Get<float>("animation_spawn_warp_time_scale");
        }

        public int GetNumberOfValidCenters()
        {
            return Count;
        }

        public float GetMinimumValidRadius()
        {
            return 50;
        }
    }
}
