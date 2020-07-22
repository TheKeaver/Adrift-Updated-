using Audrey;
using System;

namespace GameJam.Components
{
    public class LaserEnemyComponent : IComponent, ICopyComponent
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

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            // TODO: Yowza, the Process should not be null when copying
            Entity e = GetOrMakeCopy(LaserBeamEntity);
            return new LaserEnemyComponent(e);
        }
    }
}
