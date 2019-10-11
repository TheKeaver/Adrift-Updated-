using Audrey;
using Events;
using GameJam.Components;
using GameJam.Entities;
using GameJam.Events;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace GameJam.Processes
{
    public class EntityDestructionProcess : InstantProcess
    {
        private Engine Engine;
        private Entity DestroyMe;
        public EntityDestructionProcess(Engine engine, Entity destroyMe)
        {
            DestroyMe = destroyMe;
            Engine = engine;
        }
        protected override void OnTrigger()
        {
            if (Engine.GetEntities().Contains(DestroyMe))
            {
                Engine.DestroyEntity(DestroyMe);
            }
        }
    }
}
