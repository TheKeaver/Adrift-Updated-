using GameJam.Input;

namespace GameJam
{
    /// <summary>
    /// Represents a player (of having an input method and a name).
    /// </summary>
    public class Player
    {
        public string Name
        {
            get;
            private set;
        }

        public InputMethod InputMethod
        {
            get;
            private set;
        }

        public int LobbySeatIndex
        {
            get;
            set;
        } = -1;

        public Player(string name) : this(name, null)
        {
        }

        public Player(string name, InputMethod inputMethod)
        {
            Name = name;
            InputMethod = inputMethod;
        }
    }
}
