using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{

    class RespawnEntityEvent : Event
    {
        // Event Message Types
        public const string RESPAWN_ENTITY = "respawn_entity";

        public RespawnEntityEvent(string type, int recipientID)
            : base(type, recipientID)
        {
        }
    }

}
