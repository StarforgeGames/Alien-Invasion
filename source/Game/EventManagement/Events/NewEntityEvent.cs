﻿namespace Game.EventManagement.Events
{

    public class NewEntityEvent : Event
    {
        // Event Message Types
        public const string NEW_ENTITY = "new_entity";

        public int EntityID { get; private set; }

        public NewEntityEvent(string type, int entityID)
            : base(type)
        {
            this.EntityID = entityID;
        }

        public override string ToString()
        {
            return base.ToString() + " [EntityID: " + EntityID + "]";
        }
    }

}
