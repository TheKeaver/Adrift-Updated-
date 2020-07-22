using Audrey;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Components
{
    public interface ICopyComponent
    {
        IComponent Copy(Func<Entity, Entity> GetOrMakeCopy);
    }
}
