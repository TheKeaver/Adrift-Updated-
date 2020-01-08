using Audrey;
using GameJam.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes.Enemies
{
    public class SpawnPatternManager : IntervalProcess
    {
        readonly Engine Engine;
        readonly MTRandom random = new MTRandom();
        readonly ProcessManager ProcessManager;

        Dictionary<int, List<Type>> allPatternsList; // This is a Dictionary of all created SpawnPatterns
        Dictionary<int,List<Type>> patternStaleList; // This is the current list of patterns that have been used recently

        int maxDifficulty = 0; // This keeps track of the max difficulty in a genearted spawn pattern
        int difficultyCounter = 0; // This keeps track of how many times OnTick() has been called

        Process process = null;

        public SpawnPatternManager(Engine engine, ProcessManager processManager) : base(2.0f)
        {
            Engine = engine;
            ProcessManager = processManager;

            allPatternsList = GenerateAllPatternsList();
            patternStaleList = new Dictionary<int, List<Type>>();

            Interval = 5;
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
            returnDict.Add(5, new List<Type>());

            // All level 1 spawn patterns
            //returnDict[1].Add(typeof(SpawnChasingTriangle));
            // All level 2 spawn patterns
            //returnDict[2].Add(typeof(SpawnShootingTrianlge));
            // All level 3 spawn patterns
            //returnDict[3].Add(typeof(SpawnLaserTriangle));
            // All level 4 spawn patterns
            //returnDict[4].Add(typeof(SpawnChasingCircle));
            // All level 5 spawn patterns
            //returnDict[5].Add(typeof(SpawnChasingBorder));
            return returnDict;
        }

        private void GenerateSpawnPattern(int difVal)
        {
            int tempDif = difVal;
            while (tempDif > 0)
            {
                switch (tempDif % 5)
                {
                    case 0:
                        tempDif -= 5;
                        GenerateLevel5Pattern();
                        break;
                    case 1:
                        tempDif -= 1;
                        GenerateLevel1Pattern();
                        break;
                    case 2:
                        tempDif -= 2;
                        GenerateLevel2Pattern();
                        break;
                    case 3:
                        tempDif -= 3;
                        GenerateLevel3Pattern();
                        break;
                    default:
                        tempDif -= 4;
                        GenerateLevel4Pattern();
                        break;
                }
            }
            // Attach the global process to the Processmanager
            ProcessManager.Attach(process);
        }

        private void GenerateLevel1Pattern()
        {
            if (allPatternsList[1].Count == 0)
            {
                allPatternsList[1] = patternStaleList[1];
                patternStaleList.Clear();
            }

            int ran = random.Next(0, allPatternsList[1].Count);

            if(process == null)
            {
                process = (Process)Activator.CreateInstance(allPatternsList[1][ran], new object[] {});
            }
            else
            {
                process.SetNext((Process)Activator.CreateInstance(allPatternsList[1][ran], new object[] { }));
            }

            patternStaleList[1].Add(allPatternsList[1][ran]);
            allPatternsList[1].RemoveAt(ran);
        }

        private void GenerateLevel2Pattern()
        {
            throw new NotImplementedException();
        }

        private void GenerateLevel3Pattern()
        {
            throw new NotImplementedException();
        }

        private void GenerateLevel4Pattern()
        {
            throw new NotImplementedException();
        }

        private void GenerateLevel5Pattern()
        {
            throw new NotImplementedException();
        }
    }
}
