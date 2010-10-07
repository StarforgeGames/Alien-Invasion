using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{

    public abstract class Event : EventArgs
    {
        public string Type { get; set; }

        public Event(string type)
        {
            this.Type = type;
        }
    }

}
