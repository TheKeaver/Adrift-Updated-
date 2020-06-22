using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Processes
{
    public class WaitForFamilyCountProcess : Process
    {
        private int FamilyCountMaximum;

        readonly Engine Engine;
        readonly Family WaitingFamily;
        readonly ImmutableList<Entity> familyEntityList;

        public WaitForFamilyCountProcess(Engine engine, Family waitingFamily, int familyCountMaximum)
        {
            Engine = engine;
            WaitingFamily = waitingFamily;
            FamilyCountMaximum = familyCountMaximum;

            familyEntityList = engine.GetEntitiesFor(WaitingFamily);
        }
        protected override void OnInitialize()
        {
        }

        protected override void OnKill()
        {
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnUpdate(float dt)
        {
            if(familyEntityList.Count < FamilyCountMaximum)
            {
                this.Kill();
            }
        }
    }
}
