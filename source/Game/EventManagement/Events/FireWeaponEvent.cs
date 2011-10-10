namespace Game.EventManagement.Events
{

    public class FireWeaponEvent : Event
    {
        // Event Message Types
        public const string START_FIRING = "actor_start_firing";
        public const string FIRING_WEAPON = "actor_firing_weapon";
        public const string STOP_FIRING = "actor_stop_firing";
        public const string FIRE_SINGLE_SHOT = "fire_single_shot";

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
