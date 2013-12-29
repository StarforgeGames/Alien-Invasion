namespace Game.EventManagement.Events
{

    public class FireWeaponEvent : Event
    {
        // Event Message Types
        public const string START_FIRING = "ACTOR_START_FIRING";
        public const string FIRING_WEAPON = "ACTOR_FIRING_WEAPON";
        public const string STOP_FIRING = "ACTOR_STOP_FIRING";
        public const string FIRE_SINGLE_SHOT = "FIRE_SINGLE_SHOT";

        public FireWeaponEvent(string type, int recipientID)
            : base(type, recipientID)
        { }

        public static FireWeaponEvent Start(int recipientID)
        {
            return new FireWeaponEvent(START_FIRING, recipientID);
        }

        public static FireWeaponEvent Stop(int recipientID)
        {
            return new FireWeaponEvent(STOP_FIRING, recipientID);
        }

        public static FireWeaponEvent IsFiring(int recipientID)
        {
            return new FireWeaponEvent(FIRING_WEAPON, recipientID);
        }

        public static FireWeaponEvent SingleShot(int recipientID)
        {
            return new FireWeaponEvent(FIRE_SINGLE_SHOT, recipientID);
        }
    }

}
