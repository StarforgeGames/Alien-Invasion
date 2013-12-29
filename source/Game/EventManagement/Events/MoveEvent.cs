using Game.Utility;
namespace Game.EventManagement.Events
{

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public class MoveEvent : Event
    {
        // Event Message Types
        public const string START_MOVING = "ACTOR_START_MOVING";
        public const string STOP_MOVING = "ACTOR_STOP_MOVING";

        public Vector2D Direction { get; set; }

        public MoveEvent(string type, int recipientID, Vector2D direction)
            : base(type, recipientID)
        {
            this.Direction = direction;
        }

        public static MoveEvent Start(int recipientID, Vector2D direction)
        {
            return new MoveEvent(START_MOVING, recipientID, direction);
        }

        public static MoveEvent Stop(int recipientID, Vector2D direction)
        {
            return new MoveEvent(STOP_MOVING, recipientID, direction);
        }

        public override string ToString()
        {
            return base.ToString() + " [Direction: " + Direction + "]";
        }
    }

}
