using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using SharpDX.Direct2D1;
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

            if (emc.mirrorPosition == true && emc.mirrorRotation == true)
            {
               float translatedRotation = (float)(Math.PI / 2) + emc.entityToMirror.GetComponent<TransformComponent>().Rotation;

                float opp = (CVars.Get<float>("animation_trail_width") / 2) * (float)Math.Sin(translatedRotation);
                float adj = (CVars.Get<float>("animation_trail_width") / 2) * (float)Math.Cos(translatedRotation);

                Vector2 set = emc.entityToMirror.GetComponent<TransformComponent>().Position + emc.positionOffsetVector;

                set.X += opp;
                set.Y += adj;

                mirroringEntity.GetComponent<TransformComponent>().SetPosition(set);
                mirroringEntity.GetComponent<TransformComponent>().SetRotation(emc.entityToMirror.GetComponent<TransformComponent>().Rotation);

                Console.WriteLine("Current Entity Position: " + mirroringEntity.GetComponent<TransformComponent>().Position);
                Console.WriteLine("Mirrored Entity Position: " + emc.entityToMirror.GetComponent<TransformComponent>().Position);
                Console.WriteLine("Translated Offset: " + set);
            }
            else if (emc.mirrorPosition == true)
            {
                mirroringEntity.GetComponent<TransformComponent>().SetPosition(emc.entityToMirror.GetComponent<TransformComponent>().Position + emc.positionOffsetVector);
            }
            else if (emc.mirrorRotation == true)
            {
                mirroringEntity.GetComponent<TransformComponent>().SetRotation(emc.entityToMirror.GetComponent<TransformComponent>().Rotation);
            }
        }
    }
}
