using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnChasingCircle : InstantProcess, ISpawnPattern
    {
        private int radius = 200;
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
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            Center = simulatedCenters[0];

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 180), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 180)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y - 180), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y - 180)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 180, Center.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 180, Center.Y)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 180, Center.Y), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 180, Center.Y)));

            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 128, Center.Y + 128), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 128, Center.Y + 128)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 128, Center.Y - 128), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 128, Center.Y - 128)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 128, Center.Y + 128), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 128, Center.Y + 128)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 128, Center.Y - 128), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 128, Center.Y - 128)));
        }

        public float GetMaxSpawnTimer()
        {
            return CVars.Get<float>("animation_chasing_enemy_spawn_duration");
        }

        public int GetNumberOfValidCenters()
        {
            return 1;
        }

        public float GetMinimumValidRadius()
        {
            return 200;
        }
    }
}