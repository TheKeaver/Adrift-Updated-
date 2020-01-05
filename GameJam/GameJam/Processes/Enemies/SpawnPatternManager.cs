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

        public SpawnPatternManager(Engine engine, ProcessManager processManager) : base(2.0f)
        {
            Engine = engine;
            ProcessManager = processManager;
        }

        protected override void OnTick(float interval)
        {
            throw new NotImplementedException();
        }
    }
}
