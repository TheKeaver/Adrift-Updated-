using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnChasingTriangle : InstantProcess, ISpawnPattern
    {
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
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            Center = simulatedCenters[0];
            
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 25)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 29, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 29, Center.Y - 25)));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 29, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 29, Center.Y - 25)));
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
            return 100;
        }
    }
}
