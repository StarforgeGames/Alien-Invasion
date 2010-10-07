using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{
    class DeathEvent : Event
    {
        // Event Message Types
        public const string ACTOR_DIES = "actor_dies";

        public DeathEvent(string type)
            : base(type)
        {
        }
    }
}
