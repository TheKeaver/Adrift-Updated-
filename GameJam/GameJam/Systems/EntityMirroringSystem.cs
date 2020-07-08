using Audrey;
using GameJam.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Systems
{
    public class EntityMirroringSystem : BaseSystem
    {
        readonly Family _mirroringFamily = Family.All(typeof(EntityMirroringComponent)).Get();
        readonly ImmutableList<Entity> _mirroringEntities;

        public EntityMirroringSystem(Engine engine) : base(engine)
        {
            _mirroringEntities = engine.GetEntitiesFor(_mirroringFamily);
        }

        public override void Update(float dt)
        {
            foreach (Entity mirroringEntity in _mirroringEntities)
            {
                ProcessMirroring(mirroringEntity);
            }
        }

        private void ProcessMirroring(Entity mirroringEntity)
        {
            EntityMirroringComponent emc = mirroringEntity.GetComponent<EntityMirroringComponent>();

            if (emc.mirrorPosition == true)
            {
                mirroringEntity.GetComponent<TransformComponent>().SetPosition(emc.entityToMirror.GetComponent<TransformComponent>().Position + emc.positionOffsetVector);
            }
            if (emc.mirrorRotation == true)
            {
                mirroringEntity.GetComponent<TransformComponent>().SetRotation(emc.entityToMirror.GetComponent<TransformComponent>().Rotation);
            }
        }
    }
}
