namespace Game.EventManagement.Events
{

    public class DestroyEntityEvent : Event
    {
        // Event Message Types
        public const string DESTROY_ENTITY = "destroy_entity";

        public int EntityID { get; private set; }
        public int DestroyedByEntityID { get; private set; }

        public DestroyEntityEvent(string type, int entityID, int destroyedByEntityID = 0)
            : base(type)
        {
            this.EntityID = entityID;
            this.DestroyedByEntityID = destroyedByEntityID;
        }
    }

}
