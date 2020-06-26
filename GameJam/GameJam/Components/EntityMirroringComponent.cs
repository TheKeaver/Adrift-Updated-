using Audrey;
using Microsoft.Xna.Framework;

namespace GameJam.Components
{
    public class EntityMirroringComponent : IComponent
    {
        public Entity entityToMirror;
        public Vector2 positionOffsetVector;
        public bool mirrorPosition;
        public bool mirrorRotation;

        public EntityMirroringComponent(Entity toMirror, bool pos, bool rot, Vector2 offset = new Vector2())
        {
            entityToMirror = toMirror;
            mirrorPosition = pos;
            mirrorRotation = rot;
            positionOffsetVector = offset;
        }
    }
}
