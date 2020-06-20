using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomLaserEnemies : InstantProcess
    {
        private int radius = 50;

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
            for (int i = 0; i < 3; i++)
            {
                Vector2 spawnPosition = SPM.GenerateValidCenter(radius);
                float angle = SPM.AngleFacingNearestPlayerShip(spawnPosition);
                LaserEnemyEntity.Spawn(Engine, ProcessManager, spawnPosition, angle);
            }
        }
    }
}
