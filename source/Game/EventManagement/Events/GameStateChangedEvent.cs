namespace Game.EventManagement.Events
{

    public class GameStateChangedEvent : Event
    {
        // Event Message Types
        public const string GAME_STATE_CHANGED = "game_state_changed";

        public GameState NewState { get; set; }

        public GameStateChangedEvent(string type, GameState newState)
            : base(type)
        {
            this.NewState = newState;
        }

        public static GameStateChangedEvent To(GameState state)
        {
            return new GameStateChangedEvent(GAME_STATE_CHANGED, state);
        }

        public override string ToString()
        {
            return base.ToString() + " [NewState: " + NewState + "]";
        }
    }

}
