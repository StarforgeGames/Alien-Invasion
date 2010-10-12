using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{

    public class DestroyEntityEvent : Event
    {
        // Event Message Types
        public const string DESTROY_ENTITY = "destroy_entity";

        public int EntityID { get; private set; }

        public DestroyEntityEvent(string type, int entityID)
            : base(type)
        {
            this.EntityID = entityID;
        }
    }

}
