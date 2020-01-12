using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes.SpawnPatterns;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace GameJam.Processes.Enemies
{
    public class SpawnPatternManager : IntervalProcess
    {
        readonly Engine Engine;
        readonly MTRandom random = new MTRandom();
        readonly ProcessManager ProcessManager;

        Dictionary<int, List<Type>> allPatternsList; // This is a Dictionary of all created SpawnPatterns
        Dictionary<int,List<Type>> patternStaleList; // This is the current list of patterns that have been used recently

        //int maxDifficulty = 0; // This keeps track of the max difficulty in a genearted spawn pattern
        int difficultyCounter = 0; // This keeps track of how many times OnTick() has been called

        Process process = null;

        readonly Family _playerShipFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> _playerShipEntities;

        readonly Family _enemyFamily = Family.All(typeof(EnemyComponent)).Exclude(typeof(ProjectileComponent)).Get();
        readonly ImmutableList<Entity> _enemyEntities;

        public SpawnPatternManager(Engine engine, ProcessManager processManager) : base(2.0f)
        {
            Engine = engine;
            ProcessManager = processManager;

            _playerShipEntities = Engine.GetEntitiesFor(_playerShipFamily);
            _enemyEntities = Engine.GetEntitiesFor(_enemyFamily);

            patternStaleList = new Dictionary<int, List<Type>>();
            allPatternsList = GenerateAllPatternsList();

            Interval = 3;
        }

        protected override void OnTick(float interval)
        {
            // Organize spawn patterns based off of difficulty
            // Increase the difficutly counter every time OnTick is called
            difficultyCounter += 1;
            GenerateSpawnPattern(difficultyCounter);
        }

        private Dictionary<int, List<Type>> GenerateAllPatternsList()
        {
            Dictionary<int, List<Type>> returnDict = new Dictionary<int, List<Type>>();
            returnDict.Add(1, new List<Type>());
            returnDict.Add(2, new List<Type>());
            returnDict.Add(3, new List<Type>());
            returnDict.Add(4, new List<Type>());
            // '0' represents the 5th level of difficulty
            returnDict.Add(0, new List<Type>());
            //returnDict.Add(5, new List<Type>());

            // All level 1 spawn patterns
            returnDict[1].Add(typeof(SpawnChasingTriangle));
            // All level 2 spawn patterns
            //returnDict[2].Add(typeof(SpawnShootingTrianlge));
            // All level 3 spawn patterns
            //returnDict[3].Add(typeof(SpawnLaserTriangle));
            // All level 4 spawn patterns
            //returnDict[4].Add(typeof(SpawnChasingCircle));
            // All level 5 spawn patterns
            //returnDict[5].Add(typeof(SpawnChasingBorder));

            // Initialize the patternStaleList dictionary TODO: Move this somewhere else (optional)
            patternStaleList.Add(0, new List<Type>());
            patternStaleList.Add(1, new List<Type>());
            patternStaleList.Add(2, new List<Type>());
            patternStaleList.Add(3, new List<Type>());
            patternStaleList.Add(4, new List<Type>());

            return returnDict;
        }

        private void GenerateSpawnPattern(int difVal)
        {
            //Console.Write("Dif Val: " + difVal);
            int tempDif = difVal;
            while (tempDif > 0)
            {
                int val = tempDif % 5;
                tempDif -= val;
                Console.WriteLine("Pass In Val " + val);
                //GeneratePattern(val);
                tempDif += GeneratePattern(val, 0);
            }
            // Attach the global process to the Processmanager
            if(process != null)
                ProcessManager.Attach(process);
            process = null;
        }

        //private void /*int*/ GeneratePattern(int val) //, int counter)
        private int GeneratePattern(int val, int counter)
        {
            Console.WriteLine("Val: " + val);
            // If all patterns of 'val' level are stale, swap allPatternsList and patternStaleList
            if (allPatternsList[val].Count == 0)
            {
                allPatternsList[val] = patternStaleList[val];
                patternStaleList[val].Clear();
            }
            // If all patterns of 'val' are still empty then they are not implemented, simply return
            if (allPatternsList[val].Count == 0)
            {
                //return; 
                return GeneratePattern(val-1, counter + 1);
            }

            int ran = random.Next(0, allPatternsList[val].Count-1);

            if (process == null)
            {
                process = (Process)Activator.CreateInstance(allPatternsList[val][ran], new object[] {Engine, ProcessManager, this });
                process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));
            }
            else
            {
                process.SetNext((Process)Activator.CreateInstance(allPatternsList[val][ran], new object[] {Engine, ProcessManager, this }));
                process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));
            }

            patternStaleList[val].Add(allPatternsList[val][ran]);
            allPatternsList[val].RemoveAt(ran);

            return 0;
        }

        public Vector2 GenerateValidCenter(int radius)
        {
            Vector2 spawnPosition = new Vector2(0, 0);

            do
            {
                spawnPosition.X = random.NextSingle(-CVars.Get<float>("screen_width") / 2 + radius, CVars.Get<float>("screen_width") / 2 - radius);
                spawnPosition.Y = random.NextSingle(-CVars.Get<float>("screen_height") / 2 + radius, CVars.Get<float>("screen_height") / 2 - radius);
            } while (IsTooCloseToPlayer(spawnPosition, radius));

            return spawnPosition;
        }

        bool IsTooCloseToPlayer(Vector2 position, int radius)
        {
            float minDistanceToPlayer = float.MaxValue;

            foreach (Entity playerShip in _playerShipEntities)
            {
                TransformComponent transformComponent = playerShip.GetComponent<TransformComponent>();
                Vector2 toPlayer = transformComponent.Position - position;
                if (toPlayer.Length() < minDistanceToPlayer)
                {
                    minDistanceToPlayer = toPlayer.Length();
                }
            }

            return minDistanceToPlayer <= radius;
        }
    }
}
