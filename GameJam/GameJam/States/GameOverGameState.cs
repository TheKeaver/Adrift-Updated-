namespace GameJam.States
{
    public class GameOverGameState : CommonGameState
    {
        public GameOverGameState(GameManager gameManager, SharedGameState sharedState) : base(gameManager, sharedState)
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnKill()
        {
            base.OnKill();
        }

        protected override void OnRender(float dt, float betweenFrameAlpha)
        {
            base.OnRender(dt, betweenFrameAlpha);
        }

        protected override void RegisterListeners()
        {
            base.RegisterListeners();
        }

        protected override void UnregisterListeners()
        {
            base.UnregisterListeners();
        }
    }
}
