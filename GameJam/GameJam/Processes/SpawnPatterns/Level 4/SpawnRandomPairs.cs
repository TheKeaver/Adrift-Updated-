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
            // If Gravity hole is re-implemented, make it so that the two gravity holes are either
            // 1) randomly placed, or 2) Mirrored from each-other
            /*Center = SPM.GenerateValidCenter(radius);
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y));*/

            Center = SPM.GenerateValidCenter(radius);
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));

            Center = SPM.GenerateValidCenter(radius);
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));

            Center = SPM.GenerateValidCenter(radius);
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
        }
    }
}
