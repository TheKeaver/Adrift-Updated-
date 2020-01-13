using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnChasingCircle : InstantProcess
    {
        private int radius = 100;
        private Vector2 Center;

        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnChasingCircle(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            //Console.WriteLine("Triggered SpawnChasingTrianlge");
            Center = SPM.GenerateValidCenter(radius);

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 45), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 45)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y - 45), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y - 45)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 45, Center.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 45, Center.Y)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 45, Center.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 45, Center.Y)));

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 32, Center.Y + 32), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 32, Center.Y + 32)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 32, Center.Y - 32), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 32, Center.Y - 32)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 32, Center.Y + 32), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 32, Center.Y + 32)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 32, Center.Y - 32), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 32, Center.Y - 32)));
        }
    }
}