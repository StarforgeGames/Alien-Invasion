using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Messages
{
    class DeathMessage : Message
    {
        // Event Message Types
        public const string ACTOR_DIES = "actor_dies";

        public DeathMessage(string type)
            : base(type)
        {
        }
    }
}
