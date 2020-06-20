using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnShootingTriangle : InstantProcess
    {
        private int radius = 100;
        private Vector2 Center;

        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnShootingTriangle(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            //Console.WriteLine("Triggered SpawnShootingTriangle");
            Center = SPM.GenerateValidCenter(radius);

            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 50)));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 50, Center.Y - 50)));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 50, Center.Y - 50)));
        }
    }
}
