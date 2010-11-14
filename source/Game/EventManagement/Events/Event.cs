using System;

namespace Game.EventManagement.Events
{

    public abstract class Event : EventArgs
    {
        public string Type { get; set; }
        public int RecipientID { get; private set; }

        public Event(string type)
        {
            this.Type = type;
            this.RecipientID = 0;
        }

        public Event(string type, int recipientID)
            : this(type)
        {
            this.RecipientID = recipientID;
        }

        public override string ToString()
        {
            return Type + " (RecipientID: " + RecipientID + ")";
        }
    }

}
