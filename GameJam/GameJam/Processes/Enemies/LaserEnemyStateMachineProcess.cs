using Audrey;
using Audrey.Events;
using Events;
using GameJam.Common;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events.EnemyActions;
using Microsoft.Xna.Framework;
using System;

namespace GameJam.Processes.Enemies
{
    enum LaserEnemyState
    {
        Track,
        WarmUp,
        WaitToFire,
        Fire
    }

    class LaserEnemyStateMachineProcess : StateMachineProcess<LaserEnemyState>
    {
        public readonly Engine Engine;
        public readonly Entity Entity;

        public LaserEnemyStateMachineProcess(Engine engine, Entity entity) : base(LaserEnemyState.Track)
        {
            Engine = engine;
            Entity = entity;

            AddState(LaserEnemyState.Track, new TrackState(this));
            AddState(LaserEnemyState.WarmUp, new WarmUpState(this));
            AddState(LaserEnemyState.WaitToFire, new WaitToFireState(this));
            AddState(LaserEnemyState.Fire, new FireState(this));
        }

        protected override void OnTogglePause()
        {
        }

        protected override void OnKill()
        {
            LaserEnemyComponent laserEnemyComp = Entity.GetComponent<LaserEnemyComponent>();
            if (laserEnemyComp != null)
            {
                Entity laserBeamEntity = laserEnemyComp.LaserBeamEntity;
                if (laserBeamEntity != null)
                {
                    Engine.DestroyEntity(laserBeamEntity);
                }
            }
        }

        #region TRACK STATE
        private class TrackState : State
        {
            readonly LaserEnemyStateMachineProcess LaserEnemyStateMachineProcess;

            public TrackState(LaserEnemyStateMachineProcess laserEnemyStateMachineProcess)
            {
                LaserEnemyStateMachineProcess = laserEnemyStateMachineProcess;
            }

            public override Condition[] CreateConditions()
            {
                return new Condition[] {
                    new TrackToWarmUpCondition()
                };
            }

            public override void OnEnter(LaserEnemyState previousState)
            {
            }

            public override void OnExit(LaserEnemyState nextState)
            {
            }

            public override void Update(float dt)
            {
                if (LaserEnemyStateMachineProcess.Entity == null
                    || !LaserEnemyStateMachineProcess.Entity.HasComponent<LaserEnemyComponent>())
                {
                    LaserEnemyStateMachineProcess.Kill();
                    return;
                }
            }

            #region TRACK STATE CONDITIONS
            private class TrackToWarmUpCondition : Condition
            {
                private Timer _timer;

                public TrackToWarmUpCondition()
                {
                    _timer = new Timer(CVars.Get<float>("laser_enemy_successive_wait_period"));
                }

                public override bool HasConditionMet()
                {
                    return _timer.HasElapsed();
                }

                public override LaserEnemyState GetStateToSwitchTo()
                {
                    return LaserEnemyState.WarmUp;
                }

                public override void Update(float dt)
                {
                    _timer.Update(dt);
                }
            }
            #endregion
        }
        #endregion

        #region WARM UP STATE
        private class WarmUpState : State
        {
            readonly LaserEnemyStateMachineProcess LaserEnemyStateMachineProcess;

            Timer _timer;

            public WarmUpState(LaserEnemyStateMachineProcess laserEnemyStateMachineProcess)
            {
                LaserEnemyStateMachineProcess = laserEnemyStateMachineProcess;

                _timer = new Timer(0);
            }

            public override Condition[] CreateConditions()
            {
                return new Condition[] {
                    new WarmUpToWaitToFireCondition()
                };
            }

            public override void OnEnter(LaserEnemyState previousState)
            {
                LaserEnemyStateMachineProcess.Entity.GetComponent<RotationComponent>().RotationSpeed = 0;

                // Create laser beam if the laser enemy doesn't currently have one
                LaserEnemyComponent laserEnemyComp = LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>();
                TransformComponent transformComp = LaserEnemyStateMachineProcess.Entity.GetComponent<TransformComponent>();
                if (laserEnemyComp.LaserBeamEntity == null)
                {
                    laserEnemyComp.LaserBeamEntity = LaserBeamEntity.Create(LaserEnemyStateMachineProcess.Engine, transformComp.Position, false);
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

                _timer.Reset(CVars.Get<float>("laser_enemy_warm_up_anim_duration"));
            }

            public override void OnExit(LaserEnemyState nextState)
            {
            }

            public override void Update(float dt)
            {
                _timer.Update(dt);

                UpdateLaserBeamWarmUp(MathHelper.Min(_timer.Alpha, 1));
            }

            private void UpdateLaserBeamWarmUp(float alpha)
            {
                LaserEnemyComponent laserEnemyComp = LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>();
                LaserBeamComponent laserBeamComp = laserEnemyComp.LaserBeamEntity.GetComponent<LaserBeamComponent>();
                laserBeamComp.Thickness = MathHelper.Lerp(0, CVars.Get<float>("laser_enemy_warm_up_thickness"), Easings.QuadraticEaseIn(alpha));
            }

            #region WARM UP CONDITIONS
            private class WarmUpToWaitToFireCondition : Condition
            {
                private Timer _timer;

                public WarmUpToWaitToFireCondition()
                {
                    _timer = new Timer(CVars.Get<float>("laser_enemy_warm_up_anim_duration"));
                }

                public override bool HasConditionMet()
                {
                    return _timer.HasElapsed();
                }

                public override LaserEnemyState GetStateToSwitchTo()
                {
                    return LaserEnemyState.WaitToFire;
                }

                public override void Update(float dt)
                {
                    _timer.Update(dt);
                }
            }
            #endregion
        }
        #endregion

        #region WAIT TO FIRE STATE
        private class WaitToFireState : State
        {
            readonly LaserEnemyStateMachineProcess LaserEnemyStateMachineProcess;

            public WaitToFireState(LaserEnemyStateMachineProcess laserEnemyStateMachineProcess)
            {
                LaserEnemyStateMachineProcess = laserEnemyStateMachineProcess;
            }

            public override Condition[] CreateConditions()
            {
                return new Condition[]
                {
                    new WaitToFireToFireCondition()
                };
            }

            public override void OnEnter(LaserEnemyState previousState)
            {
            }

            public override void OnExit(LaserEnemyState nextState)
            {
            }

            public override void Update(float dt)
            {
                if (LaserEnemyStateMachineProcess.Entity == null
                    || !LaserEnemyStateMachineProcess.Entity.HasComponent<LaserEnemyComponent>()
                    || LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>().LaserBeamEntity == null)
                {
                    LaserEnemyStateMachineProcess.Kill();
                    return;
                }
            }

            #region WARM TO FIRE CONDITIONS
            private class WaitToFireToFireCondition : Condition
            {
                private Timer _timer;

                public WaitToFireToFireCondition()
                {
                    _timer = new Timer(CVars.Get<float>("laser_enemy_fire_duration"));
                }

                public override bool HasConditionMet()
                {
                    return _timer.HasElapsed();
                }

                public override LaserEnemyState GetStateToSwitchTo()
                {
                    return LaserEnemyState.Fire;
                }

                public override void Update(float dt)
                {
                    _timer.Update(dt);
                }
            }
            #endregion
        }
        #endregion

        #region FIRE STATE
        private class FireState : State
        {
            Timer _timer;

            readonly LaserEnemyStateMachineProcess LaserEnemyStateMachineProcess;

            float _initialBeamThickness;

            public FireState(LaserEnemyStateMachineProcess laserEnemyStateMachineProcess)
            {
                LaserEnemyStateMachineProcess = laserEnemyStateMachineProcess;

                _timer = new Timer(0);
            }

            public override Condition[] CreateConditions()
            {
                return new Condition[]
                {
                    new FireToTrackCondition()
                };
            }

            public override void OnEnter(LaserEnemyState previousState)
            {
                // Create laser beam if the laser enemy doesn't currently have one
                LaserEnemyComponent laserEnemyComp = LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>();
                TransformComponent transformComp = LaserEnemyStateMachineProcess.Entity.GetComponent<TransformComponent>();
                if (laserEnemyComp.LaserBeamEntity == null)
                {
                    laserEnemyComp.LaserBeamEntity = LaserBeamEntity.Create(LaserEnemyStateMachineProcess.Engine, transformComp.Position, true);
                    return;
                }

                LaserBeamComponent laserBeamComp = laserEnemyComp.LaserBeamEntity.GetComponent<LaserBeamComponent>();
                _initialBeamThickness = laserBeamComp.Thickness;
                laserBeamComp.InteractWithShield = true;
                laserBeamComp.ComputeReflection = true;

                // Add CollisionComponent if the laser beam doesn't have one
                if (!laserEnemyComp.LaserBeamEntity.HasComponent<CollisionComponent>())
                {
                    laserEnemyComp.LaserBeamEntity.AddComponent(new CollisionComponent(new PolygonCollisionShape(new Vector2[] {
                    new Vector2(10, -10),
                    new Vector2(10, 10),
                    new Vector2(-10, 10),
                    new Vector2(-10, -10)
                })));
                }

                EventManager.Instance.QueueEvent(new LaserBeamFireStart(laserEnemyComp.LaserBeamEntity));

                _timer.Reset(CVars.Get<float>("laser_enemy_fire_duration"));
            }

            public override void OnExit(LaserEnemyState nextState)
            {
                // Destroy laser beam
                LaserEnemyComponent laserEnemyComp = LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>();
                if (laserEnemyComp != null)
                {
                    if (laserEnemyComp.LaserBeamEntity != null)
                    {
                        LaserEnemyStateMachineProcess.Engine.DestroyEntity(laserEnemyComp.LaserBeamEntity);
                        laserEnemyComp.LaserBeamEntity = null;
                        EventManager.Instance.QueueEvent(new LaserBeamFireEnd(laserEnemyComp.LaserBeamEntity));
                    }
                }

                LaserEnemyStateMachineProcess.Entity.GetComponent<RotationComponent>().RotationSpeed = CVars.Get<float>("laser_enemy_rotational_speed");
            }

            public override void Update(float dt)
            {
                if(LaserEnemyStateMachineProcess.Entity == null
                    || !LaserEnemyStateMachineProcess.Entity.HasComponent<LaserEnemyComponent>()
                    || LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>().LaserBeamEntity == null)
                {
                    LaserEnemyStateMachineProcess.Kill();
                }

                _timer.Update(dt);

                UpdateLaserBeamFire(MathHelper.Min(_timer.Alpha, 1));
            }

            private void UpdateLaserBeamFire(float alpha)
            {
                LaserEnemyComponent laserEnemyComp = LaserEnemyStateMachineProcess.Entity.GetComponent<LaserEnemyComponent>();
                LaserBeamComponent laserBeamComp = laserEnemyComp.LaserBeamEntity.GetComponent<LaserBeamComponent>();

                float frequency = CVars.Get<float>("laser_enemy_fire_frequency");
                float offset = CVars.Get<float>("laser_enemy_fire_thickness");
                float zeroToPeak = CVars.Get<float>("laser_enemy_fire_thickness_variability");
                float initialThicknessDecaySpeed = CVars.Get<float>("laser_enemy_fire_initial_thickness_decay_factor");
                float closingEnvelopeDecaySpeed = CVars.Get<float>("laser_enemy_fire_closing_envelope_decay_factor");

                float thickness = (float)(-zeroToPeak * Math.Sin(2 * MathHelper.Pi * frequency * _timer.Elapsed)) + MathHelper.Lerp(_initialBeamThickness, offset, 1 - (float)Math.Exp(-initialThicknessDecaySpeed * 5 * alpha));
                thickness *= (float)(1 - Math.Exp(-closingEnvelopeDecaySpeed * 5 * (1 - alpha))); // Closing envelope
                laserBeamComp.Thickness = thickness;
            }

            #region FIRE CONDITIONS
            private class FireToTrackCondition : Condition
            {
                private Timer _timer;

                public FireToTrackCondition()
                {
                    _timer = new Timer(CVars.Get<float>("laser_enemy_fire_duration"));
                }

                public override bool HasConditionMet()
                {
                    return _timer.HasElapsed();
                }

                public override LaserEnemyState GetStateToSwitchTo()
                {
                    return LaserEnemyState.Track;
                }

                public override void Update(float dt)
                {
                    _timer.Update(dt);
                }
            }
            #endregion
        }
        #endregion
    }
}
