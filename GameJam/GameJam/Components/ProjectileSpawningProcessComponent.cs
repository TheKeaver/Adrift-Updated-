using Audrey;
using System;

namespace GameJam.Components
{
    public class ProjectileSpawningProcessComponent : IComponent, ICopyComponent
    {
        public  Process FiringProcess;

        public ProjectileSpawningProcessComponent(Process firingProcess)
        {
            this.FiringProcess = firingProcess;
        }

        // TODO: This is hwack, Process needs to be moved outside of the Component
        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new ProjectileSpawningProcessComponent(null);
        }
    }
}
