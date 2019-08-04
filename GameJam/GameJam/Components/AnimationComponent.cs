using Audrey;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;

namespace GameJam.Components
{
    public class AnimationComponent : IComponent
    {
        public SpriteSheetAnimationFactory AnimationFactory;
        public AnimatedSprite[] Animations;
        public int ActiveAnimationIndex = -1;

        public AnimationComponent(SpriteSheetAnimationFactory animationFactory,
            AnimatedSprite[] animations)
        {
            AnimationFactory = animationFactory;
            Animations = animations;
        }
    }
}
