namespace Game.EventManagement.Events
{

    public class DamageEvent : Event
    {
        // Event Message Types
        public const string RECEIVE_DAMAGE = "ACTOR_RECEIVE_DAMAGE";

        public int Damage { get; set; }
        public int SourceEntityID { get; set; }

        public DamageEvent(string type, int damage, int recipientID, int sourceEntityID)
            : base(type, recipientID)
        {
            this.Damage = damage > 0 ? damage : 0;  // Only positive amounts are valid
            this.SourceEntityID = sourceEntityID;
        }

        public static DamageEvent Receive(int damage, int recipientID, int sourceEntityID)
        {
            return new DamageEvent(RECEIVE_DAMAGE, damage, recipientID, sourceEntityID);
        }

        public override string ToString()
        {
            return base.ToString() + " [Damage: " + Damage + ", SourceEntityID: " + SourceEntityID + "]";
        }
    }

}
