namespace Game.EventManagement.Events
{

    public class CollisionEvent : Event
    {
        // Event Message Types
        public const string ACTOR_COLLIDES = "actor_collides";

        public int OtherEntityID { get; set; }

        public CollisionEvent(string type, int recipientID, int otherEntityID)
            : base(type, recipientID)
        {
            this.OtherEntityID = otherEntityID;
        }

        public static CollisionEvent Collides(int recipientID, int otherEntityID)
        {
            return new CollisionEvent(ACTOR_COLLIDES, recipientID, otherEntityID);
        }

        public override string ToString()
        {
            return base.ToString() + " [OtherEntityID: " + OtherEntityID + "]";
        }
    }

}
