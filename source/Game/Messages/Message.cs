using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Messages
{

    public abstract class Message
    {
        public string Type { get; set; }

        public Message(string type)
        {
            this.Type = type;
        }
    }

}
