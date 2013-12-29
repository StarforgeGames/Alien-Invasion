namespace Game.EventManagement.Events
{

    public class DestroyEntityEvent : Event
    {
        // Event Message Types
        public const string DESTROY_ENTITY = "DESTROY_ENTITY";

        public int EntityID { get; private set; }
        public int DestroyedByEntityID { get; private set; }

        public DestroyEntityEvent(string type, int entityID, int destroyedByEntityID = 0)
            : base(type)
        {
            this.EntityID = entityID;
            this.DestroyedByEntityID = destroyedByEntityID;
        }

        public static DestroyEntityEvent Destroy(int entityID, int destroyedByEntityID = 0)
        {
            return new DestroyEntityEvent(DESTROY_ENTITY, entityID, destroyedByEntityID);
        }
    }

}
