using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnChasingTriangle : InstantProcess
    {
        private int radius = 400;
        private Vector2 Center;

        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnChasingTriangle(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            Center = SPM.GenerateValidCenter(radius);

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 250), 0);
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 290, Center.Y - 250), 0);
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 290, Center.Y - 250), 0);
        }
    }
}
