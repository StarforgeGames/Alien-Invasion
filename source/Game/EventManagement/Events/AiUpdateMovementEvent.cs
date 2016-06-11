using Game.Utility;

namespace Game.EventManagement.Events
{
    public class AiUpdateMovementEvent : Event
    {
         // Event Message Types
        public const string AT_BORDER = "AT_BORDER";

        public int EntityID { get; private set; }
        public Vector2D BorderData { get; private set; }

        public AiUpdateMovementEvent(string type, int entityID, Vector2D borderData)
            : base(type)
        {
            this.EntityID = entityID;
            this.BorderData = borderData;
        }

        public static AiUpdateMovementEvent AtBorder(int entityID, Vector2D borderData) {
            return new AiUpdateMovementEvent(AT_BORDER, entityID, borderData);
        }
    }
}
