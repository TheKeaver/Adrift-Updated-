﻿using System;
using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Menu
{
    public class EntityBackgroundSpawner : IntervalProcess
    {
        public Engine Engine
        {
            get;
            private set;
        }

        public Camera Camera
        {
            get;
            private set;
        }

        private readonly MTRandom _random;

        private Func<Engine, Entity>[] _entityCreationFunctions;

        public EntityBackgroundSpawner(Engine engine, Camera camera)
            :base(CVars.Get<float>("entity_background_spawner_min"))
        {
            Engine = engine;
            Camera = camera;
            _random = new MTRandom();

            _entityCreationFunctions = new Func<Engine, Entity>[] {
                ChasingEnemyEntity.CreateSpriteOnly,
                LaserEnemyEntity.CreateSpriteOnly,
                ShootingEnemyEntity.CreateSpriteOnly
            };
        }

        protected override void OnTick(float dt)
        {
            SpawnEntity();
            Interval = _random.NextSingle(CVars.Get<float>("entity_background_spawner_min"),
                CVars.Get<float>("entity_background_spawner_max"));
        }

        private void SpawnEntity()
        {
            float zoom = Camera.TotalZoom;

            Entity entity = _entityCreationFunctions[_random.Next(0, _entityCreationFunctions.Length - 1)].Invoke(Engine);
            TransformComponent transformComp = entity.GetComponent<TransformComponent>();
            transformComp.SetPosition(CVars.Get<float>("entity_background_spawner_x") / zoom,
                _random.NextSingle(CVars.Get<float>("entity_background_spawner_y_min"),
                    CVars.Get<float>("entity_background_spawner_y_max")), true);
            transformComp.SetRotation((float)(_random.NextSingle() * Math.PI * 2), true);
            entity.AddComponent(new MovementComponent(new Vector2(-1, 0),
                _random.NextSingle(CVars.Get<float>("entity_background_entity_speed_min"),
                    CVars.Get<float>("entity_background_entity_speed_max"))));
            entity.GetComponent<MovementComponent>().UpdateRotationWithDirection = false;
            entity.AddComponent(new RotationComponent(_random.NextSingle(CVars.Get<float>("entity_background_entity_angular_speed_min"),
                CVars.Get<float>("entity_background_entity_angular_speed_max"))));
            entity.AddComponent(new MenuBackgroundComponent());
        }
    }
}
