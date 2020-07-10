using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public class VectorSpriteTrailEntity 
    {
        public static Entity Create(Engine engine, Entity shipEntity)
        {
            Entity entity = engine.CreateEntity();
            // "offset" stores the mid point of both ends of the player trail
            Vector2 offset = new Vector2(-3, 0);

            TransformComponent shipTransform = shipEntity.GetComponent<TransformComponent>();
            //offset += shipTransform.Position;
            entity.AddComponent(new VectorSpriteTrailComponent(shipEntity, CVars.Get<float>("animation_trail_width")));
            entity.AddComponent(new TransformComponent(shipTransform.Position + offset));
            entity.AddComponent(new TransformHistoryComponent(offset, shipTransform.Rotation, 50));
            entity.AddComponent(new EntityMirroringComponent(shipEntity, true, true, offset ));

            return entity;
        }
    }
}
