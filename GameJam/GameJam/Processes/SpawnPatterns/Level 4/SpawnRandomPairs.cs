using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnRandomPairs : InstantProcess, ISpawnPattern
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
            Vector2[] simulatedCenters = SPM.BeginSimulation(GetMaxSpawnTimer(), GetNumberOfValidCenters(), GetMinimumValidRadius());

            // If Gravity hole is re-implemented, make it so that the two gravity holes are either
            // 1) randomly placed, or 2) Mirrored from each-other
            /*Center = SPM.GenerateValidCenter(radius);
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y));*/

            Center = simulatedCenters[0];
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
            ChasingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));

            Center = simulatedCenters[1];
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
            ShootingEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));

            Center = simulatedCenters[2];
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X + 50, Center.Y - 50), SPM.AngleFacingNearestPlayerShip(Center));
            LaserEnemyEntity.Spawn(Engine, ProcessManager, new Vector2(Center.X - 50, Center.Y + 50), SPM.AngleFacingNearestPlayerShip(Center));
        }

        public float GetMaxSpawnTimer()
        {
            float chase = CVars.Get<float>("animation_chasing_enemy_spawn_duration");
            float warp = (CVars.Get<float>("animation_spawn_warp_phase_1_base_duration") + CVars.Get<float>("animation_spawn_warp_phase_2_base_duration"))
                   * CVars.Get<float>("animation_spawn_warp_time_scale");
            return Math.Max(chase, warp);
        }

        public int GetNumberOfValidCenters()
        {
            return 3;
        }

        public float GetMinimumValidRadius()
        {
            return 100;
        }
    }
}
