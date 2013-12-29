namespace Game.EventManagement.Events
{

	class RespawnEntityEvent : Event
	{
		// Event Message Types
		public const string RESPAWN_ENTITY = "RESPAWN_ENTITY";

		public RespawnEntityEvent(string type, int recipientID)
			: base(type, recipientID)
		{ }

		public static RespawnEntityEvent Respawn(int recipientID)
		{
			return new RespawnEntityEvent(RESPAWN_ENTITY, recipientID);
		}
	}

}
