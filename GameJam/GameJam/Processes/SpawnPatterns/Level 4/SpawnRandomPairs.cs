using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomPairs : InstantProcess
    {
        private int radius = 100;
        Vector2 Center;

        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnRandomPairs(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            Center = SPM.GenerateValidCenter(radius);
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y));

            Center = SPM.GenerateValidCenter(radius);
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));

            Center = SPM.GenerateValidCenter(radius);
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 25, Center.Y + 25), SPM.AngleFacingNearestPlayerShip(Center));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 25, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(Center));

            Center = SPM.GenerateValidCenter(radius);
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 25, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(Center));
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 25, Center.Y + 25), SPM.AngleFacingNearestPlayerShip(Center));
        }
    }
}
