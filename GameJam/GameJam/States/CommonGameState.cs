namespace GameJam.States
{
    public class CommonGameState : GameState
    {
        public SharedGameState SharedState
        {
            get;
            private set;
        }

        public CommonGameState(GameManager gameManager,
            SharedGameState sharedState) : base(gameManager)
        {
            SharedState = sharedState;
        }
    }
}
