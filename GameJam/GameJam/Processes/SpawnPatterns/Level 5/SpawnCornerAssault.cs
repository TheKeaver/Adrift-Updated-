using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnCornerAssault : IntervalProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public int spawnLimit = 36;
        public int spawnCount;
        public bool spawnedUniqueEnemies;

        private Vector2 topLeft;
        private Vector2 topRight;
        private Vector2 botLeft;
        private Vector2 botRight;

        public SpawnCornerAssault(Engine engine, ProcessManager processManager, SpawnPatternManager spm) : base ( 1.0f )
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;

            float maxWidth = CVars.Get<float>("play_field_width");
            float maxHeight = CVars.Get<float>("play_field_height");

            topLeft = new Vector2(-maxWidth / 2 + 50, maxHeight / 2 - 50);
            topRight = new Vector2(maxWidth / 2 - 50, maxHeight / 2 - 50);
            botLeft = new Vector2(-maxWidth / 2 + 50, -maxHeight / 2 + 50);
            botRight = new Vector2(maxWidth / 2 - 50, -maxHeight / 2 + 50);
        }

        public float GetMaxSpawnTimer()
        {
            return 0;
        }

        public float GetMinimumValidRadius()
        {
            return 150;
        }

        public int GetNumberOfValidCenters()
        {
            return 0;
        }

        protected override void OnTick(float interval)
        {
            if (!spawnedUniqueEnemies)
            {
                if (!SPM.IsTooCloseToPlayer(topLeft, GetMinimumValidRadius()))
                    ShootingEnemyEntity.Spawn(Engine, ProcessManager, topLeft, SPM.AngleFacingNearestPlayerShip(topLeft));
                
                if (!SPM.IsTooCloseToPlayer(topRight, GetMinimumValidRadius()))
                    ShootingEnemyEntity.Spawn(Engine, ProcessManager, topRight, SPM.AngleFacingNearestPlayerShip(topRight));
                
                if (!SPM.IsTooCloseToPlayer(botLeft, GetMinimumValidRadius()))
                    ShootingEnemyEntity.Spawn(Engine, ProcessManager, botLeft, SPM.AngleFacingNearestPlayerShip(botLeft));
                
                if (!SPM.IsTooCloseToPlayer(botRight, GetMinimumValidRadius()))
                    ShootingEnemyEntity.Spawn(Engine, ProcessManager, botRight, SPM.AngleFacingNearestPlayerShip(botRight));
                
                spawnedUniqueEnemies = true;
                spawnCount += 4;
            }

            if (!SPM.IsTooCloseToPlayer(topLeft, GetMinimumValidRadius()))
            {
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(topLeft.X + 100, topLeft.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(topLeft.X + 100, topLeft.Y)));
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(topLeft.X, topLeft.Y - 100), SPM.AngleFacingNearestPlayerShip(new Vector2(topLeft.X, topLeft.Y - 100)));
            }

            if (!SPM.IsTooCloseToPlayer(topRight, GetMinimumValidRadius()))
            {
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(topRight.X - 100, topRight.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(topRight.X - 100, topRight.Y)));
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(topRight.X, topRight.Y - 100), SPM.AngleFacingNearestPlayerShip(new Vector2(topRight.X, topRight.Y - 100)));
            }

            if (!SPM.IsTooCloseToPlayer(botLeft, GetMinimumValidRadius()))
            {
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(botLeft.X + 100, botLeft.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(botLeft.X - 100, botLeft.Y)));
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(botLeft.X, botLeft.Y + 100), SPM.AngleFacingNearestPlayerShip(new Vector2(botLeft.X, botLeft.Y + 100)));
            }

            if (!SPM.IsTooCloseToPlayer(botRight, GetMinimumValidRadius()))
            {
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(botRight.X - 100, botRight.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(botRight.X - 100, botRight.Y)));
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(botRight.X, botRight.Y + 100), SPM.AngleFacingNearestPlayerShip(new Vector2(botRight.X, botRight.Y + 100)));
            }

            spawnCount += 8;

            if (spawnCount >= spawnLimit)
                Kill();
        }
    }
}
