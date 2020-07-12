using Audrey;

namespace GameJam.Components
{
    public class LaserEnemyComponent : IComponent
    {
        public Entity LaserBeamEntity
        {
            get;
            set;
        } = null;

        public Process LaserEnemyStateMachineProcess
        {
            get;
            set;
        } = null;

        public LaserEnemyComponent(Entity laserBeamEntity = null)
        {
            LaserBeamEntity = laserBeamEntity;
        }
    }
}
