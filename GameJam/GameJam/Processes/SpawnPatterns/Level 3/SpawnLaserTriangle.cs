using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnLaserTriangle : InstantProcess, ISpawnPattern
    {
        private int radius = 100;
        private Vector2 Center;

        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public SpawnLaserTriangle(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;
        }

        protected override void OnTrigger()
        {
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            Center = simulatedCenters[0];

            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X, Center.Y + 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X, Center.Y + 25)));
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 29, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X - 29, Center.Y - 25)));
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 29, Center.Y - 25), SPM.AngleFacingNearestPlayerShip(new Vector2(Center.X + 29, Center.Y - 25)));
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
            return 150;
        }
    }
}