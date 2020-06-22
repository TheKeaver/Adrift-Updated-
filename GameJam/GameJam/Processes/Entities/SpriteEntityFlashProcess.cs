using Audrey;
using GameJam.Components;

namespace GameJam.Processes.Entities
{
    public class SpriteEntityFlashProcess : IntervalProcess
    {
        public int FlashesRemaining
        {
            get;
            set;
        }

        public Engine Engine
        {
            get;
            private set;
        }

        public Entity Entity {
            get;
            private set;
        }

        public SpriteEntityFlashProcess(Engine engine, Entity entity, int flashes, float halfPeriod) : base(halfPeriod)
        {
            FlashesRemaining = flashes;
            Engine = engine;
            Entity = entity;
        }

        protected override void OnTick(float interval)
        {
            if(!Engine.GetEntities().Contains(Entity))
            {
                Kill();
                return;
            }

            bool flashed = false;
            SpriteComponent spriteComp = Entity.GetComponent<SpriteComponent>();
            if(spriteComp != null)
            {
                spriteComp.Hidden = !spriteComp.Hidden;
                if (!spriteComp.Hidden)
                {
                    flashed = true;
                }
            }
            VectorSpriteComponent vectorSpriteComp = Entity.GetComponent<VectorSpriteComponent>();
            if(vectorSpriteComp != null)
            {
                vectorSpriteComp.Hidden = !vectorSpriteComp.Hidden;
                if(!vectorSpriteComp.Hidden)
                {
                    flashed = true;
                }
            }

            if(flashed)
            {
                FlashesRemaining--;
            }
            if(FlashesRemaining <= 0)
            {
                Kill();
            }
        }
    }
}
