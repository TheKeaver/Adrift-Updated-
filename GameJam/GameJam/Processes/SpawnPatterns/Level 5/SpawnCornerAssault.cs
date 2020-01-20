using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnCornerAssault : IntervalProcess
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public float maxWidth;
        public float maxHeight;

        public int spawnLimit = 36;
        public int spawnCount;
        public bool spawnedUniqueEnemies;

        public SpawnCornerAssault(Engine engine, ProcessManager processManager, SpawnPatternManager spm) : base ( 1.0f )
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;

            maxWidth = CVars.Get<float>("play_field_width");
            maxHeight = CVars.Get<float>("play_field_height");
        }

        protected override void OnTick(float interval)
        {
            if (!spawnedUniqueEnemies)
            {
                /*LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2 (-maxWidth / 2, maxHeight / 2)));
                LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2 (-maxWidth / 2, maxHeight / 2)));
                LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2 (-maxWidth / 2, maxHeight / 2)));
                LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2 (-maxWidth / 2, maxHeight / 2)));*/
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
                ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
                spawnedUniqueEnemies = true;
                spawnCount += 4;
            }

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 150, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, maxHeight / 2 - 150), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 150, maxHeight / 2 - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, maxHeight / 2 - 150), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 150, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + 50, -maxHeight / 2 + 150), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 150, -maxHeight / 2 + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - 50, -maxHeight / 2 + 150), SPM.AngleFacingNearestPlayerShip(new Vector2(-maxWidth / 2, maxHeight / 2)));
            spawnCount += 8;

            if (spawnCount >= spawnLimit)
                Kill();
        }
    }
}
