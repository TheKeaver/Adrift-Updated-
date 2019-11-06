using Audrey;
using GameJam.Components;

namespace GameJam.Systems
{
    public class AnimationSystem : BaseSystem
    {
        readonly Family _animatedSprites = Family.All(typeof(AnimationComponent)).Get();
        readonly ImmutableList<Entity> _animatedEntities;

        public AnimationSystem(Engine engine) : base(engine)
        {
            _animatedEntities = engine.GetEntitiesFor(_animatedSprites);
        }

        protected override void OnUpdate(float dt)
        {
            foreach(Entity animatedEntity in _animatedEntities)
            {
                AnimationComponent animationComp = animatedEntity.GetComponent<AnimationComponent>();
                if(animationComp.ActiveAnimationIndex > -1)
                {
                    animationComp.Animations[animationComp.ActiveAnimationIndex].Update(dt);
                }
            }
        }
    }
}
