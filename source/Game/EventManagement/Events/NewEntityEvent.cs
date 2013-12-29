namespace Game.EventManagement.Events
{

    public class NewEntityEvent : Event
    {
        // Event Message Types
        public const string NEW_ENTITY = "NEW_ENTITY";

        public int EntityID { get; private set; }

        public NewEntityEvent(string type, int entityID)
            : base(type)
        {
            this.EntityID = entityID;
        }

        public static NewEntityEvent Announce(int entityID)
        {
            return new NewEntityEvent(NEW_ENTITY, entityID);
        }

        public override string ToString()
        {
            return base.ToString() + " [EntityID: " + EntityID + "]";
        }
    }

}
