using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomShootingEnemies : InstantProcess
    {
        private int radius = 50;

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
            for (int i = 0; i < Count; i++)
            {
                Vector2 spawnPosition = SPM.GenerateValidCenter(radius);
                float angle = SPM.AngleFacingNearestPlayerShip(spawnPosition);
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, spawnPosition, angle);
            }
        }
    }
}
