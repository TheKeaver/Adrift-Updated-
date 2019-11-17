using Audrey;
using Events;
using GameJam.Events.EnemyActions;
using GameJam.Processes.Common;
using Microsoft.Xna.Framework.Content;

namespace GameJam.Directors
{
    public class ExplosionDirector : BaseDirector
    {
        private readonly ParallelExplosionParticleGenerationProcess _parallelExplosionParticleGenerationProcess;

        public ExplosionDirector(Engine engine, ContentManager content,
            ProcessManager processManager,
            ParallelExplosionParticleGenerationProcess parallelExplosionParticleGenerationProcess)
            :base(engine, content, processManager)
        {
            _parallelExplosionParticleGenerationProcess = parallelExplosionParticleGenerationProcess;
        }

        protected override void RegisterEvents()
        {
            EventManager.Instance.RegisterListener<CreateExplosionEvent>(this);
        }

        protected override void UnregisterEvents()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public override bool Handle(IEvent evt)
        {
            if(evt is CreateExplosionEvent)
            {
                CreateExplosionEvent createExplosionEvent = evt as CreateExplosionEvent;
                _parallelExplosionParticleGenerationProcess.QueueExplosion(new ColoredExplosion {
                    Position = createExplosionEvent.ExplosionLocation,
                    Color = createExplosionEvent.Color
                });
            }
            return false;
        }
    }
}
