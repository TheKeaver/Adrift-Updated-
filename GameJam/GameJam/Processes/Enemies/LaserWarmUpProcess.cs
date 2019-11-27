using Audrey;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events.EnemyActions;
using Microsoft.Xna.Framework;

namespace GameJam.Processes.Enemies
{
    public class LaserWarmUpProcess : Process
    {
        private readonly Engine Engine;
        private Entity LaserEnemyEntity;

        private Timer Timer;

        public LaserWarmUpProcess(Engine engine, Entity laserEnemyEntity)
        {
            Engine = engine;
            LaserEnemyEntity = laserEnemyEntity;

            Timer = new Timer(CVars.Get<float>("laser_enemy_warm_up_anim_duration"));
        }

        protected override void OnInitialize()
        {
            if (!Engine.GetEntities().Contains(LaserEnemyEntity))
            {
                SetNext(null);
                Kill();
                return;
            }

            // Create laser beam if the laser enemy doesn't currently have one
            LaserEnemyComponent laserEnemyComp = LaserEnemyEntity.GetComponent<LaserEnemyComponent>();
            TransformComponent transformComp = LaserEnemyEntity.GetComponent<TransformComponent>();
            if (laserEnemyComp.LaserBeamEntity == null)
            {
                laserEnemyComp.LaserBeamEntity = LaserBeamEntity.Create(Engine, transformComp.Position, false);
            }

            LaserBeamComponent laserBeamComp = laserEnemyComp.LaserBeamEntity.GetComponent<LaserBeamComponent>();
            laserBeamComp.Thickness = 0;
            laserBeamComp.InteractWithShield = true;
            laserBeamComp.ComputeReflection = false;

            // Remove CollisionComponent if the laser beam has one
            if (laserEnemyComp.LaserBeamEntity.HasComponent<CollisionComponent>())
            {
                laserEnemyComp.LaserBeamEntity.RemoveComponent<CollisionComponent>();
            }

            EventManager.Instance.QueueEvent(new LaserBeamWarmUpStart(laserEnemyComp.LaserBeamEntity));
        }

        protected override void OnKill()
        {
            if (Engine.GetEntities().Contains(LaserEnemyEntity) && LaserEnemyEntity.HasComponent<LaserEnemyComponent>())
            {
                EventManager.Instance.QueueEvent(new LaserBeamWarmUpEnd(LaserEnemyEntity.GetComponent<LaserEnemyComponent>().LaserBeamEntity));
            }
        }

        protected override void OnTogglePause()
        {

        }

        protected override void OnUpdate(float dt)
        {
            if(!Engine.GetEntities().Contains(LaserEnemyEntity))
            {
                SetNext(null);
                Kill();
                return;
            }

            Timer.Update(dt);
            if(Timer.HasElapsed())
            {
                Kill();
                return;
            }

            UpdateLaserBeam(MathHelper.Min(Timer.Alpha, 1));
        }

        private void UpdateLaserBeam(float alpha)
        {
            LaserEnemyComponent laserEnemyComp = LaserEnemyEntity.GetComponent<LaserEnemyComponent>();
            LaserBeamComponent laserBeamComp = laserEnemyComp.LaserBeamEntity.GetComponent<LaserBeamComponent>();
            laserBeamComp.Thickness = MathHelper.Lerp(0, CVars.Get<float>("laser_enemy_warm_up_thickness"), Easings.QuadraticEaseIn(alpha));
        }
    }
}
