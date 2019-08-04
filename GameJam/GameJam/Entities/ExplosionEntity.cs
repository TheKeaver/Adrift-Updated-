using System;
using System.Collections.Generic;
using Audrey;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;

namespace GameJam.Entities
{
    public static class ExplosionEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.EXPLOSION_BOUNDS));
            entity.AddComponent(new ExplosionComponent());

            Dictionary<string, Rectangle> explMap = new Dictionary<string, Rectangle>();
            explMap.Add("Explosion1", new Rectangle(0, 0, 32, 32));
            explMap.Add("Explosion2", new Rectangle(32, 0, 32, 32));
            explMap.Add("Explosion3", new Rectangle(64, 0, 32, 32));
            explMap.Add("Explosion4", new Rectangle(96, 0, 32, 32));
            explMap.Add("Explosion5", new Rectangle(128, 0, 32, 32));
            explMap.Add("Explosion6", new Rectangle(160, 0, 32, 32));
            explMap.Add("Explosion7", new Rectangle(192, 0, 32, 32));
            TextureAtlas explAtlas = new TextureAtlas("explosion", texture, explMap);
            SpriteSheetAnimationFactory explAnimationFactory = new SpriteSheetAnimationFactory(explAtlas);
            explAnimationFactory.Add("default", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4, 5, 6 }, isLooping: false, frameDuration: 0.1f));
            AnimatedSprite explAnim = new AnimatedSprite(explAnimationFactory);
            entity.AddComponent(new Components.AnimationComponent(explAnimationFactory, new AnimatedSprite[] { explAnim }));
            entity.GetComponent<Components.AnimationComponent>().ActiveAnimationIndex = 0;
            explAnim.Play("default", () =>
            {
                entity.GetComponent<Components.AnimationComponent>().ActiveAnimationIndex = -1;
            });

            return entity;
        }
    }
}
