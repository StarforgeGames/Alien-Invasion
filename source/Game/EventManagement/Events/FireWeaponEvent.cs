using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{

    public class FireWeaponEvent : Event
    {
        // Event Message Types
        public const string START_FIRING = "actor_start_firing";
        public const string FIRING_WEAPON = "actor_firing_weapon";
        public const string STOP_FIRING = "actor_stop_firing";

        public FireWeaponEvent(string type, int recipientID)
            : base(type, recipientID)
        {
        }
    }

}
