using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ProjectileSpawningProcessComponent : IComponent
    {
        public  Process FiringProcess;

        public ProjectileSpawningProcessComponent(Process firingProcess)
        {
            this.FiringProcess = firingProcess;
        }
    }
}
