using Audrey;
using GameJam.Entities;
using GameJam.Processes.Enemies;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.SpawnPatterns
{
    public class SpawnChasingBorder : IntervalProcess, ISpawnPattern
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly SpawnPatternManager SPM;

        public int spawnNumber = 0;
        public float spacingInterval = 200;
        public float horizontalSpacing;
        public float verticalSpacing;

        public Vector2 originalSpawnLocation;
        public Vector2 nextSpawnLocation;
        public Vector2 spawnerDirection = new Vector2(1, 0);

        public float maxWidth;
        public float maxHeight;

        public SpawnChasingBorder(Engine engine, ProcessManager processManager, SpawnPatternManager spm) : base ( 0.5f )
        {
            Engine = engine;
            ProcessManager = processManager;
            SPM = spm;

            maxWidth = CVars.Get<float>("play_field_width");
            maxHeight = CVars.Get<float>("play_field_height");

            horizontalSpacing = (maxWidth % spacingInterval) / 2;
            verticalSpacing = (maxHeight % spacingInterval) / 2;

            originalSpawnLocation = new Vector2(-maxWidth / 2 + horizontalSpacing, maxHeight / 2 - verticalSpacing);
            nextSpawnLocation = new Vector2(-maxWidth / 2 + horizontalSpacing, maxHeight / 2 - verticalSpacing);
        }

        public float GetMaxSpawnTimer()
        {
            return 0;
        }

        public float GetMinimumValidRadius()
        {
            return 100;
        }

        public int GetNumberOfValidCenters()
        {
            return 0;
        }

        protected override void OnTick(float interval)
        {
            if (!SPM.IsTooCloseToPlayer(nextSpawnLocation, GetMinimumValidRadius()))
            {
                ChasingEnemyEntity.Spawn(Engine, ProcessManager, nextSpawnLocation, SPM.AngleFacingNearestPlayerShip(nextSpawnLocation));
                spawnNumber += 1;
            }

            if (spawnNumber > 2 && (nextSpawnLocation == originalSpawnLocation))
            {
                Kill();
            }

            if (spawnerDirection.X != 0)
            {
                nextSpawnLocation.X += spawnerDirection.X * spacingInterval;
                if( nextSpawnLocation.X > maxWidth / 2 || nextSpawnLocation.X < - maxWidth / 2)
                {
                    nextSpawnLocation = new Vector2(spawnerDirection.X * maxWidth / 2 - (spawnerDirection.X * horizontalSpacing),
                                                    spawnerDirection.X * maxHeight / 2 - (spawnerDirection.X * (verticalSpacing + spacingInterval)));
                    spawnerDirection = new Vector2(0, -spawnerDirection.X);
                }
            }
            else if (spawnerDirection.Y != 0)
            {
                nextSpawnLocation.Y +=  spawnerDirection.Y * spacingInterval;
                if( nextSpawnLocation.Y > maxHeight / 2 || nextSpawnLocation.Y < - maxHeight / 2)
                {
                    nextSpawnLocation = new Vector2(spawnerDirection.Y * -maxWidth / 2 + (spawnerDirection.Y * (horizontalSpacing + spacingInterval)),
                                                    spawnerDirection.Y * maxHeight / 2 - (spawnerDirection.Y * verticalSpacing));

                    spawnerDirection = new Vector2(spawnerDirection.Y, 0);
                }
            }
        }
    }
}
