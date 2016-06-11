using Game.EventManagement.Events;

namespace Game.EventManagement.Debug
{
    public class DebugEvent : Event
    {
        // Event Message Types
        public const string SINGLE_STEP = "execute_single_step";
        public const string INCREASE_SPEED = "increase_game_speed";
        public const string DECREASE_SPEED = "decrease_game_speed";
        public const string RESET_SPEED = "reset_game_speed";

        public DebugEvent(string type)
            : base(type)
        { }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
