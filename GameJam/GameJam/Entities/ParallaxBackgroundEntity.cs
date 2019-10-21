﻿using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Entities
{
    public static class ParallaxBackgroundEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 origin, float strength, bool pulsing = false)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(origin));
            entity.AddComponent(new SpriteComponent(texture,
                new Vector2(texture.Width, texture.Height)));
            entity.GetComponent<SpriteComponent>().RenderGroup = Constants.Render.RENDER_GROUP_STARS;
            if (pulsing)
            {
                entity.AddComponent(new PulseComponent(15, 0.75f, 1));
            }
            entity.AddComponent(new ParallaxBackgroundComponent(strength, origin));

            return entity;
        }
    }
}