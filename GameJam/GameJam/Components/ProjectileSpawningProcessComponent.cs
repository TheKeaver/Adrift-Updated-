using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ProjectileSpawningProcessComponent : IComponent
    {
        public FirePorjectileProcess firingProcess;

        public ProjectileSpawningProcessComponent(FirePorjectileProcess theProcess)
        {
            firingProcess = theProcess;
        }
    }
}
