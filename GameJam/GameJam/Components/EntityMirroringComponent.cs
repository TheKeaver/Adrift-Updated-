using Audrey;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Components
{
    public class EntityMirroringComponent : IComponent, ICopyComponent
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

        public IComponent Copy(Func<Entity, Entity> GetOrMakeCopy)
        {
            Entity e = GetOrMakeCopy(entityToMirror);
            return new EntityMirroringComponent(e, mirrorPosition, mirrorRotation, new Vector2(positionOffsetVector.X, positionOffsetVector.Y));
        }
    }
}
