using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomGravityHoles : InstantProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnRandomGravityHoles(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            // TODO: Implement Random Gravity Holes
            for (int i = 0; i < 2; i++)
            {
                Vector2 spawnPosition = SPM.GenerateValidCenter(GetMinimumValidRadius());
                GravityHoleEntity.Spawn(Engine, ProcessManager, spawnPosition);
            }
        }

        public float GetMaxSpawnTimer()
        {
            return 0;
        }

        public int GetNumberOfValidCenters()
        {
            // TODO: Make generating valid Gravity based on proximity of other gravity
            // Return 0 should skip the valid centers code and just execute
            return 0;
        }

        public float GetMinimumValidRadius()
        {
            return 50;
        }
    }
}
