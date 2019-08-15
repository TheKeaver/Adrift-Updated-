using Audrey;
using GameJam.Common;
using GameJam.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam.Entities
{
    public static class KamikazeEntity
    {
        public static Entity Create(Engine engine, Texture2D texture, Vector2 position)
        {
            Entity entity = engine.CreateEntity();

            entity.AddComponent(new TransformComponent(position));
            entity.AddComponent(new SpriteComponent(texture, Constants.ObjectBounds.KAMIKAZE_SHIP_BOUNDS));
            entity.AddComponent(new RotationComponent(Constants.GamePlay.KAMIKAZE_ROTATION_SPEED));
            entity.AddComponent(new MovementComponent());
            entity.GetComponent<MovementComponent>().speed = Constants.GamePlay.KAMIKAZE_ENEMY_SPEED;
            entity.AddComponent(new EnemyComponent());
            entity.AddComponent(new CollisionComponent(new BoundingRect(0, 0, 21.875f, 21.875f)));
            entity.AddComponent(new KamikazeComponent());

            Dictionary<string, Rectangle> animMap = new Dictionary<string, Rectangle>();
            animMap.Add("2", new Rectangle(0, 0, 32, 32));
            animMap.Add("1", new Rectangle(32, 0, 32, 32));
            TextureAtlas animAtlas = new TextureAtlas("explosion", texture, animMap);
            SpriteSheetAnimationFactory animAnimationFactory = new SpriteSheetAnimationFactory(animAtlas);
            animAnimationFactory.Add("default", new SpriteSheetAnimationData(new[] { 0, 1 }, isLooping: true, frameDuration: 0.4f));
            AnimatedSprite anim = new AnimatedSprite(animAnimationFactory);
            entity.AddComponent(new Components.AnimationComponent(animAnimationFactory, new AnimatedSprite[] { anim }));
            entity.GetComponent<Components.AnimationComponent>().ActiveAnimationIndex = 0;
            anim.Play("default");

            return entity;
        }
    }
}
