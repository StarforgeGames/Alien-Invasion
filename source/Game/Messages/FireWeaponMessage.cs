using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Messages
{

    class FireWeaponMessage : Message
    {
        // Event Message Types
        public const string START_FIRING = "actor_start_firing";
        public const string STOP_FIRING = "actor_stop_firing";

        public FireWeaponMessage(Boolean isFiring)
        {
            if (isFiring) {
                Type = START_FIRING;
            }
            else {
                Type = STOP_FIRING;
            }
        }
    }

}
