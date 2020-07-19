using Audrey;
using System;

namespace GameJam.Components
{
    public class ChasingEnemyComponent : IComponent, ICopyComponent
    {
        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new ChasingEnemyComponent();
        }
    }
}
