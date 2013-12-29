namespace Game.EventManagement.Events
{

	public class DestroyEntityEvent : Event
	{
		// Event Message Types
		public const string DESTROY_ENTITY = "DESTROY_ENTITY";

		public int EntityID { get; private set; }
		public int DestroyedByEntityID { get; private set; }
		public bool IsCausedByCleanup { get; private set; }

		public DestroyEntityEvent(string type, int entityID, int destroyedByEntityID = 0, bool causedByCleanup = false)
			: base(type)
		{
			this.EntityID = entityID;
			this.DestroyedByEntityID = destroyedByEntityID;
			this.IsCausedByCleanup = causedByCleanup;
		}

		public static DestroyEntityEvent Destroy(int entityID, int destroyedByEntityID = 0, bool causedByCleanup = false)
		{
			return new DestroyEntityEvent(DESTROY_ENTITY, entityID, destroyedByEntityID, causedByCleanup);
		}
	}

}
