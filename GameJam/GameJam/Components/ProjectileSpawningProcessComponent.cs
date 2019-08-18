using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public class ProjectileSpawningProcessComponent : IComponent
    {
        public  Process firingProcess;

        public ProjectileSpawningProcessComponent(Process firingProcess)
        {
            this.firingProcess = firingProcess;
        }
    }
}
