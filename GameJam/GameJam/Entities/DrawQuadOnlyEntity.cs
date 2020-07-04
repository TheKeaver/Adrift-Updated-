using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public class DrawQuadOnlyEntity
    {
        public static Entity Create(Engine engine, Vector2 position, Vector2[] vertices, Entity referenceEntity)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));

            // Create a Quad based on the current and previous TransformHistory
            entity.AddComponent(new VectorSpriteComponent(new RenderShape[] {
                    new QuadRenderShape(
                        vertices[0],
                        vertices[1],
                        vertices[2],
                        vertices[3],
                        referenceEntity.GetComponent<ColoredExplosionComponent>().Color)
                }));
            entity.GetComponent<VectorSpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_GAME_ENTITIES;
            entity.GetComponent<TransformComponent>().SetScale(referenceEntity.GetComponent<TransformComponent>().Scale);
            
            return entity;
        }
    }
}
