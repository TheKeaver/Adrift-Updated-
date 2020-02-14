using Events;
using GameJam.Common;
using GameJam.Events.EnemyActions;
using GameJam.Particles;
using Microsoft.Xna.Framework;

namespace GameJam.States
{
    public class ParticleTestGameState : CommonGameState
    {
        private Timer _timer;
        private MTRandom _random;

        private GPUParticleManager _gpuParticleManager;

        private bool _gpuParticles = true;

        public ParticleTestGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
            _timer = new Timer(0);
            _random = new MTRandom();
        }

        protected override void OnInitialize()
        {
            _gpuParticleManager = new GPUParticleManager(GameManager.GraphicsDevice,
                Content,
                "effect_particle_update",
                "effect_particle_draw");

            base.OnInitialize();
        }

        protected override void OnUpdate(float dt)
        {
            _timer.Update(dt);
            if(_timer.HasElapsed())
            {
                Color randomColor = new HSLColor(_random.NextSingle(0, 240), 240, 120);
                if (!_gpuParticles)
                {
                    EventManager.Instance.QueueEvent(new CreateExplosionEvent(new Vector2(_random.NextSingle(-CVars.Get<float>("screen_width") / 2, CVars.Get<float>("screen_width") / 2),
                        _random.NextSingle(-CVars.Get<float>("screen_height") / 2, CVars.Get<float>("screen_height") / 2)), randomColor, false));
                    _timer.Reset(_random.NextSingle(0.04f, 0.1f));
                }
            }

            base.OnUpdate(dt);
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            _gpuParticleManager.UpdateAndDraw();

            base.OnRender(dt, betweenFrameAlpha);
        }
    }
}
