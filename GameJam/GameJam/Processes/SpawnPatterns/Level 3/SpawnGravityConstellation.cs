using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnGravityConstellation : InstantProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public float radius = CVars.Get<float>("gravity_hole_enemy_radius") * 2;
        public float maxWidth;
        public float maxHeight;

        public SpawnGravityConstellation(Engine engine, ProcessManager processManager, SpawnPatternManager spm)
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;

            maxWidth = CVars.Get<float>("play_field_width");
            maxHeight = CVars.Get<float>("play_field_height");
        }

        public static bool CanSpawn(Engine engine)
        {
            // This will be called by the spawn pattern manager, the return will need to change based on whether the pattern is active or not
            return false;
        }

        public float GetMaxSpawnTimer()
        {
            return 0;
        }

        public float GetMinimumValidRadius()
        {
            return 0;
        }

        public int GetNumberOfValidCenters()
        {
            // Returning zero should just make the spawn check pass immediately
            return 0;
        }

        protected override void OnTrigger()
        {
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + radius, maxHeight/2 - radius));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - radius, maxHeight/2 - radius));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(-maxWidth / 2 + radius, -maxHeight/2 + radius));
            GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(maxWidth / 2 - radius, -maxHeight/2 + radius));
            //GravityHoleEntity.Spawn(Engine, ProcessManager, new Vector2(0, 0));
;        }
    }
}
