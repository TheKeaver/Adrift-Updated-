using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnShootingTriangle : InstantProcess, ISpawnPattern
    {
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
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            Center = simulatedCenters[0];

            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 50)));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 50, Center.Y - 50)));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 50, Center.Y - 50)));
        }

        public float GetMaxSpawnTimer()
        {
            return (CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") + CVars.Get<float>("animation_spawn_warp_phase_2_base_duration"))
                   * CVars.Get<float>("animation_spawn_warp_time_scale");
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
