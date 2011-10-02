using System;

namespace Game.EventManagement.Events
{

    public abstract class Event : EventArgs
    {
        public string Type { get; private set; }
        // TODO: Make RecipientID a nullable Type
        public int RecipientID { get; private set; }
        // TODO: Add SourceEntityID and remove from inherited classes

        public Event(string type)
            : this(type, 0)
        { }

        public Event(string type, int recipientID)
        {
            this.Type = type;
            this.RecipientID = recipientID;
        }

        public override string ToString()
        {
            return Type + " (RecipientID: " + RecipientID + ")";
        }
    }

}
