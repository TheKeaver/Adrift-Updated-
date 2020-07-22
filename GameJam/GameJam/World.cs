using Audrey;
using Events;
using GameJam.Components;
using GameJam.Directors;
using GameJam.Systems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        /// <summary>
        /// This is saved to be loaded into the EventManager. Instance whenever
        /// a simulation begins
        /// </summary>
        public EventManager WorldLocalEventManager
        {
            get;
            private set;
        }

        BaseSystem[] _systems;

        public World(Engine engine, EventManager name)
        {
            ProcessManager = new ProcessManager();
            WorldLocalEventManager = name;
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
            EventManager real = EventManager.Instance;
            EventManager.Instance = WorldLocalEventManager;

            ProcessManager.Attach(new EnemyCollisionWithShipDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new EnemyCollisionOnShieldDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new ChangeToChasingEnemyDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new EnemyPushBackOnPlayerDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new HazardCollisionOnEnemyDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new BounceDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new LaserBeamCleanupDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new PlayerShipCollidingWithEdgeDirector(Engine, null, ProcessManager));
            ProcessManager.Attach(new SuperShieldDisplayCleanupDirector(Engine, null, ProcessManager));

            EventManager.Instance = real;
        }

        public void OnFixedUpdate(float dt)
        {
            EventManager real = EventManager.Instance;
            EventManager.Instance = WorldLocalEventManager;
            //EventManager.Instance.simulationMode = true;

            this.ProcessManager.Update(dt);

            EventManager.Instance = real;
            //EventManager.Instance.simulationMode = false;
        }

        public void CopyOtherWorldIntoThis(World other)
        { 
            // Destroy all currently stored entities while loop
            while(Engine.GetEntities().Count > 0)
            {
                Engine.DestroyEntity(Engine.GetEntities()[0]);
            }

            ImmutableList<Entity> entityList = other.Engine.GetEntities();

            // Key is the Old entity
            // Value is the New entity
            Dictionary<Entity, Entity> entityMap = new Dictionary<Entity, Entity>();

            // This function goes through one entity and makes an exact copy of the
            // entity itself
            Entity GetOrMakeEntityCopy(Entity entity)
            {
                if (entity == null)
                {
                    return null;
                }

                if (entityMap.ContainsKey(entity))
                {
                    return entityMap[entity];
                }
                else
                {
                    Entity copy = Engine.CreateEntity();
                    entityMap.Add(entity, copy);
                    ImmutableList<Audrey.IComponent> componentList = entity.GetComponents();

                    foreach (Audrey.IComponent ic in componentList)
                    {
                        if (ic is ICopyComponent)
                        {
                            copy.AddComponent(((ICopyComponent)ic).Copy(GetOrMakeEntityCopy));
                        }
                    }
                    return copy;
                }
            }

            // The below loop goes through each entity and copies over its components
            foreach (Entity e in entityList)
            {
                GetOrMakeEntityCopy(e);
            }
        }
    }
}
