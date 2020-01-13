using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Processes.SpawnPatterns;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
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

        int difficultyCounter = 0; // This keeps track of how many times OnTick() has been called

        Process process = null;
        int maxPatternDif = 5;

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
            //'0' represents the 5th level of difficulty
            //returnDict.Add(0, new List<Type>());
            returnDict.Add(5, new List<Type>());

            // All level 1 spawn patterns
            returnDict[1].Add(typeof(SpawnChasingTriangle));
            // All level 2 spawn patterns
            returnDict[2].Add(typeof(SpawnShootingTriangle));
            // All level 3 spawn patterns
            returnDict[3].Add(typeof(SpawnLaserTriangle));
            // All level 4 spawn patterns
            returnDict[4].Add(typeof(SpawnChasingCircle));
            // All level 5 spawn patterns
            //returnDict[5].Add(typeof(SpawnChasingBorder));

            // Initialize the patternStaleList dictionary TODO: Move this somewhere else (optional)
            patternStaleList.Add(1, new List<Type>());
            patternStaleList.Add(2, new List<Type>());
            patternStaleList.Add(3, new List<Type>());
            patternStaleList.Add(4, new List<Type>());
            patternStaleList.Add(5, new List<Type>());

            return returnDict;
        }

        private void GenerateSpawnPattern(int difVal)
        {
            int tempDif = difVal;
            while (tempDif > 0)
            {
                int val = (tempDif % maxPatternDif == 0) ? maxPatternDif : tempDif % maxPatternDif;
                tempDif -= val;
                int[] randomArray = GenerateRandomArray(val);
                Console.WriteLine(string.Join("", randomArray));
                foreach (int num in randomArray)
                {
                    GeneratePattern(num);
                }
            }
            // Attach the global process to the Processmanager
            if (process != null)
            {
                ProcessManager.Attach(process);
            }
            process = null;
        }

        private int[] GenerateRandomArray(int val)
        {
            ArrayList returnList = new ArrayList();
            while (val > 0)
            {
                int random = this.random.Next(1, val);
                returnList.Add(random);
                val -= random;
            }
            return (int[])returnList.ToArray(typeof(int));
        }

        private void GeneratePattern(int num)
        {
            // If all patterns of 'val' level are stale, swap allPatternsList and patternStaleList
            if (allPatternsList[num].Count == 0)
            {
                Console.WriteLine("allPatterns Count: " + allPatternsList[num].Count);
                Console.WriteLine("patternStale Count: " + patternStaleList[num].Count);
                allPatternsList[num].AddRange(patternStaleList[num]);
                patternStaleList[num].Clear();
            }
            if (allPatternsList[num].Count == 0)
            {
                return;
            }

            int ran = random.Next(0, allPatternsList[num].Count-1);

            if (process == null)
            {
                process = (Process)Activator.CreateInstance(allPatternsList[num][ran], new object[] {Engine, ProcessManager, this });
                process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));
            }
            else
            {
                process.SetNext((Process)Activator.CreateInstance(allPatternsList[num][ran], new object[] {Engine, ProcessManager, this }));
                process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));
            }

            patternStaleList[num].Add(allPatternsList[num][ran]);
            allPatternsList[num].RemoveAt(ran);
        }

        public float AngleFacingNearestPlayerShip(Vector2 position)
        {
            float minDistanceToPlayer = float.MaxValue;
            Vector2 closestPlayerShip = new Vector2(0, 0);

            foreach (Entity playerShip in _playerShipEntities)
            {
                TransformComponent transformComponent = playerShip.GetComponent<TransformComponent>();
                Vector2 toPlayer = transformComponent.Position - position;
                if (toPlayer.Length() < minDistanceToPlayer)
                {
                    minDistanceToPlayer = toPlayer.Length();
                    closestPlayerShip = toPlayer;
                }
            }

            return (float)Math.Atan2(closestPlayerShip.Y, closestPlayerShip.X);
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

        private bool IsTooCloseToPlayer(Vector2 position, int radius)
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
