﻿using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Processes.SpawnPatterns;
using GameJam.States;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GameJam.Processes.Enemies
{
    public class SpawnPatternManager : IntervalProcess
    {
        /*
         * Spawn Simulation World will copy the current SharedGameState World
         * whenever a spawn pattern is to be generated. It will copy over all
         * current Components and Directors and the like, allowing the Shared
         * Game State to be paused and kept the same.
         */
        public World SpawnSimulationWorld
        {
            get;
            private set;
        }

        public World OtherWorld
        {
            get;
            private set;
        }

        // This should always be false unless testing 1 specific Spawn Pattern
        private bool killAfterOneProcessFlag = false;
        // This should always be 1 unless testing specific Spawn Pattern
        int difficultyModifier = 1;

        readonly Engine Engine;
        readonly MTRandom random = new MTRandom();
        readonly ProcessManager ProcessManager;

        Dictionary<int, List<Type>> allPatternsList; // This is a Dictionary of all created SpawnPatterns
        Dictionary<int,List<Type>> patternStaleList; // This is the current list of patterns that have been used recently

        int difficultyCounter = 0; // This keeps track of how many times OnTick() has been called

        Process process = null;
        int maxPatternDif = 5;

        private bool _spawnedFirstStaticPattern = false;

        readonly Family _playerShipFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> _playerShipEntities;
        readonly ImmutableList<Entity> _simulationShipEntities;

        readonly Family _enemyFamily = Family.All(typeof(EnemyComponent)).Exclude(typeof(ProjectileComponent)).Get();
        readonly ImmutableList<Entity> _enemyEntities;

        public SpawnPatternManager(Engine engine, ProcessManager processManager, World otherWorld) : base(2.0f)
        {
            Engine = engine;
            ProcessManager = processManager;

            SpawnSimulationWorld = new World(new Engine(), new EventManager());
            OtherWorld = otherWorld;

            _playerShipEntities = Engine.GetEntitiesFor(_playerShipFamily);
            _enemyEntities = Engine.GetEntitiesFor(_enemyFamily);

            _simulationShipEntities = SpawnSimulationWorld.Engine.GetEntitiesFor(_playerShipFamily);

            patternStaleList = new Dictionary<int, List<Type>>();
            allPatternsList = GenerateAllPatternsList();
        }

        protected override void OnTick(float interval)
        {
            Interval = 5; // First interval is 2 seconds, then change to every 5 seconds

            if(!_spawnedFirstStaticPattern)
            {
                _spawnedFirstStaticPattern = true;
                SpawnFirstStaticPattern();
                return;
            }

            // Organize spawn patterns based off of difficulty
            // Increase the difficutly counter every time OnTick is called
            difficultyCounter += difficultyModifier;
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
            
           //// All level 1 spawn patterns
           returnDict[1].Add(typeof(SpawnChasingTriangle));
           returnDict[1].Add(typeof(SpawnRandomChasingEnemies));
           returnDict[1].Add(typeof(SpawnGravityConstellation));

           //// All level 2 spawn patterns
           returnDict[2].Add(typeof(SpawnShootingTriangle));
           //returnDict[1].Add(typeof(SpawnRandomGravityHoles));
           returnDict[2].Add(typeof(SpawnRandomShootingEnemies));
           returnDict[2].Add(typeof(SpawnGravityConstellation));

           //// All level 3 spawn patterns
           //returnDict[3].Add(typeof(SpawnLaserTriangle));
           returnDict[3].Add(typeof(SpawnGravityConstellation));
           returnDict[3].Add(typeof(SpawnRandomLaserEnemies));
           returnDict[3].Add(typeof(SpawnShootingTriangle));

           //// All level 4 spawn patterns
           returnDict[4].Add(typeof(SpawnChasingCircle));
           returnDict[4].Add(typeof(SpawnRandomPairs));
           returnDict[4].Add(typeof(SpawnGravityConstellation));

           //// All level 5 spawn patterns
           returnDict[5].Add(typeof(SpawnChasingBorder));
           returnDict[5].Add(typeof(SpawnCornerAssault));
           returnDict[5].Add(typeof(SpawnGravityConstellation));

            // Initialize the patternStaleList dictionary TODO: Move this somewhere else (optional)
            patternStaleList.Add(1, new List<Type>());
            patternStaleList.Add(2, new List<Type>());
            patternStaleList.Add(3, new List<Type>());
            patternStaleList.Add(4, new List<Type>());
            patternStaleList.Add(5, new List<Type>());

            return returnDict;
        }

        private void SpawnFirstStaticPattern()
        {
            // TODO: Make SpawnRandomChasingEnemies based on number of players (?)
            process = new SpawnRandomChasingEnemies(Engine, ProcessManager, this, 1);
            process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));

            // Attach the global process to the Processmanager
            if (process != null)
            {
                ProcessManager.Attach(process);
                if (killAfterOneProcessFlag == true)
                    this.Kill();
            }
            process = null;
        }

        private void GenerateSpawnPattern(int difVal)
        {
            //Console.WriteLine("Is in simulation mode = " + EventManager.Instance.simulationMode);
           
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
                if (killAfterOneProcessFlag == true)
                    this.Kill();
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
                //Console.WriteLine("allPatterns Count: " + allPatternsList[num].Count);
                //Console.WriteLine("patternStale Count: " + patternStaleList[num].Count);
                allPatternsList[num].AddRange(patternStaleList[num]);
                patternStaleList[num].Clear();
            }
            // If the spawn pattern list swap fails, return
            if (allPatternsList[num].Count == 0)
            {
                return;
            }

            int ran = random.Next(0, allPatternsList[num].Count-1);
            
            /* 
             * Continue resetting the generated process if
             *  1) The process is null
             *  2) The process is a "SpawnGravityConstellation" and its ".canSpawn value is true
             *  
             *  Just make sure that the SpawnGravityConstellation difficulty level has a 2nd spawn pattern in it. Otherwise its infinite.
             */  
            while (process == null || (process is SpawnGravityConstellation && SpawnGravityConstellation.canSpawn == false))
            {
                if ()
                {

                }
                else
                {
                    ran = random.Next(0, allPatternsList[num].Count - 1);
                    process = (Process)Activator.CreateInstance(allPatternsList[num][ran], new object[] { Engine, ProcessManager, this });
                }
            }
            process.SetNext(new WaitForFamilyCountProcess(Engine, _enemyFamily, CVars.Get<int>("spawner_max_enemy_count")));

            patternStaleList[num].Add(allPatternsList[num][ran]);
            allPatternsList[num].RemoveAt(ran);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="amountOfTime"></param>
       /// <returns></returns>
        public Vector2[] BeginSimulation(float amountOfTime, int numValidCenters, float minRadius)
        {
            //Console.WriteLine("Begin Simulation -> Copy Other World into this");
            
            // Load current game state into the Simulation world
            SpawnSimulationWorld.CopyOtherWorldIntoThis(OtherWorld);

            ExecuteSimulation(amountOfTime);
            
            return GenerateValidCenters(numValidCenters, minRadius, true);
        }

        private Vector2[] GenerateValidCenters(int numValidCenters, float minRadius, bool simulation = false)
        {
            //Console.WriteLine("Generating " + numValidCenters + " valid centers");
            
            Vector2[] toReturn = new Vector2[numValidCenters];
            int index = 0;

            while (index < numValidCenters)
            {
                toReturn[index] = GenerateValidCenter(minRadius, simulation);
                index++;
            }

            //Console.WriteLine("Returning " + toReturn.Length + " valid centers");
            return toReturn;
        }

        /// <summary>
        /// Called after a call to spawn an entity is made.
        /// Purprose is to simulate all of the spawning animation frames, setting
        /// up for GenerateValideCenter() that will return a valid position for spawning
        /// </summary>
        /// <param name="amountOfTime">
        /// Determined by the call to spawn a type of entity, can be derived from
        /// the CVars that define the amount of time it takes for a spawn animation
        /// </param>
        private void ExecuteSimulation(float amountOfTime)
        {
            //Console.WriteLine("Simulating for " + amountOfTime + " seconds");
            
            float elapsedTime = 0;

            while (elapsedTime < amountOfTime)
            {
                SpawnSimulationWorld.OnFixedUpdate(1 / CVars.Get<float>("spawn_simulation_tick_frequency"));
                elapsedTime += (1 / CVars.Get<float>("spawn_simulation_tick_frequency"));
            }

            return;
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

        public Vector2 GenerateValidCenter(float radius, bool simulation = false)
        {
            Vector2 spawnPosition = new Vector2(0, 0);

            do
            {
                spawnPosition.X = random.NextSingle(-CVars.Get<float>("screen_width") / 2 + radius, CVars.Get<float>("screen_width") / 2 - radius);
                spawnPosition.Y = random.NextSingle(-CVars.Get<float>("screen_height") / 2 + radius, CVars.Get<float>("screen_height") / 2 - radius);
            } while (IsTooCloseToPlayer(spawnPosition, radius, simulation));

            return spawnPosition;
        }

        // This function used to be private, changed to public to be used in SpawnChasingBorder so that
        // SpawnPatterns can check if a pre-determined location is too close to a player at the time of spawning
        // The spawn will be skipped in these scenarios
        public bool IsTooCloseToPlayer(Vector2 position, float radius, bool simulation = false)
        {
            ImmutableList<Entity> playerShips = (simulation) ? _simulationShipEntities : _playerShipEntities;

            if(playerShips.Count <= 0)
            {
                return false;
            }

            float minDistanceToPlayer = float.MaxValue;

            foreach (Entity playerShip in playerShips)
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
