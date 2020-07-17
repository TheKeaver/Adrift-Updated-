using Audrey;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameJam
{
    public class World
    {
        public ProcessManager ProcessManager
        {
            get;
            private set;
        }

        public Engine Engine
        {
            get;
            private set;
        }

        BaseSystem[] _systems;

        public World(Engine engine)
        {
            ProcessManager = new ProcessManager();
            Engine = engine;

            InitSystems();
            InitDirectors();
        }

        private void InitSystems()
        {
            _systems = new BaseSystem[]
            {
                new InputSystem(Engine),

                new TransformResetSystem(Engine),

                new PassiveRotationSystem(Engine),

                new ChasingSpeedIncreaseSystem(Engine),
                new LaserEnemySystem(Engine),
                new GravitySystem(Engine),
                new EnemySeparationSystem(Engine),
                new MovementSystem(Engine),
                new PlayerShieldSystem(Engine),
                new QuadTreeSystem(Engine),
                new SuperShieldSystem(Engine),

                new EnemyRotationSystem(Engine),
                new EntityMirroringSystem(Engine),

                new CollisionDetectionSystem(Engine)
            };

            foreach (BaseSystem sys in _systems)
            {
                ProcessManager.Attach(sys);
            }
        }

        private void InitDirectors()
        {
            ProcessManager.Attach(new EnemyCollisionWithShipDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new EnemyCollisionOnShieldDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new ChangeToChasingEnemyDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new EnemyPushBackOnPlayerDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new HazardCollisionOnEnemyDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new BounceDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new LaserBeamCleanupDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new PlayerShipCollidingWithEdgeDirector(Engine, null, ProcessManager));

            ProcessManager.Attach(new SuperShieldDisplayCleanupDirector(Engine, null, ProcessManager));
        }

        public void OnFixedUpdate(float dt)
        {
            this.ProcessManager.Update(dt);
        }

        public Entity GetOrMakeCopy(Entity e)
        {

        }
    }
}
