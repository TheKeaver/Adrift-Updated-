using Audrey;
using System;

namespace GameJam.Components
{
    public class EnemyComponent : IComponent, ICopyComponent
    {
        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            return new EnemyComponent();
        }
    }
}
