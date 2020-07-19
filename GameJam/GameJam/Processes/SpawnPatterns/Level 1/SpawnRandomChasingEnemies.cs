using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomChasingEnemies : InstantProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public int Count
        {
            get;
            private set;
        }

        public SpawnRandomChasingEnemies(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
            : this(engine, processManager, spm, 3)
        {
        }
        public SpawnRandomChasingEnemies(Engine engine, ProcessManager processManager, SpawnPatternManager spm, int count)
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
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, simulatedCenters[i], angle);
            }
        }

        public float GetMaxSpawnTimer()
        {
            return CVars.Get<float>("animation_chasing_enemy_spawn_duration");
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
