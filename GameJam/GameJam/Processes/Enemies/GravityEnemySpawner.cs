using System;
using Audrey;
using GameJam.Common;
using GameJam.Components;
using GameJam.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam.Processes
{
    public class GravityEnemySpawner : IntervalProcess
    {
        readonly Engine Engine;
        readonly ProcessManager ProcessManager;
        readonly MTRandom random = new MTRandom((uint)(0xFFFFFFFF & DateTime.Now.Ticks) + 21);

        readonly Family _playerShipFamily = Family.All(typeof(TransformComponent), typeof(PlayerShipComponent)).Get();
        readonly ImmutableList<Entity> _playerShipEntities;

        readonly Family _enemyFamily = Family.All(typeof(EnemyComponent)).Exclude(typeof(ProjectileComponent)).Get();
        readonly ImmutableList<Entity> _enemyEntities;

        public GravityEnemySpawner(Engine engine, ProcessManager processManager)
            : base(CVars.Get<float>("spawner_gravity_enemy_initial_period"))
        {
            Engine = engine;
            ProcessManager = processManager;

            _playerShipEntities = engine.GetEntitiesFor(_playerShipFamily);
            _enemyEntities = engine.GetEntitiesFor(_enemyFamily);
        }

        protected override void OnTick(float interval)
        {
            if (_enemyEntities.Count < CVars.Get<int>("spawner_max_enemy_count"))
            {
                Vector2 spawnPosition = new Vector2(0, 0);
                do
                {
                    spawnPosition.X = random.NextSingle(-CVars.Get<float>("screen_width") / 2 * 0.9f, CVars.Get<float>("screen_width") / 2 * 0.9f);
                    spawnPosition.Y = random.NextSingle(-CVars.Get<float>("screen_height") / 2 * 0.9f, CVars.Get<float>("screen_height") / 2 * 0.9f);
                } while (IsTooCloseToPlayer(spawnPosition));

                GravityHoleEntity.Create(Engine, spawnPosition, ProcessManager);
            }

            Interval = MathHelper.Max(Interval * CVars.Get<float>("spawner_gravity_enemy_period_multiplier"), CVars.Get<float>("spawner_gravity_enemy_period_min"));
        }

        bool IsTooCloseToPlayer(Vector2 position)
        {
            float minDistanceToPlayer = float.MaxValue;

            foreach (Entity playerShip in _playerShipEntities)
            {
                TransformComponent transformComponent = playerShip.GetComponent<TransformComponent>();
                Vector2 toPlayer = transformComponent.Position - position;
                if (toPlayer.Length() < minDistanceToPlayer)
                {
                    minDistanceToPlayer = toPlayer.Length();
                }
            }

            return minDistanceToPlayer <= CVars.Get<float>("spawner_min_distance_away_from_player");
        }
    }
}
