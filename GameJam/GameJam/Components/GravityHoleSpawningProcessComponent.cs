using Audrey;

namespace GameJam.Components
{
    public class GravityHoleSpawningProcessComponent : IComponent
    {
        public Process GravityProcess;

        public GravityHoleSpawningProcessComponent(Process gravityProcess)
        {
            this.GravityProcess = gravityProcess;
        }
    }
}
