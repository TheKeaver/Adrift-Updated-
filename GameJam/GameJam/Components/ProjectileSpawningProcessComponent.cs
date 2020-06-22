using Audrey;

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
