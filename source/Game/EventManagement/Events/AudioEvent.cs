using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{
    public class AudioEvent : Event
    {
         // Event Message Types
        public const string PLAY_SOUND = "play_sound";
        public const string STOP_SOUND = "stop_sound";

        public string SoundResource { get; set; }
        public int SourceEntityID { get; set; }

        public AudioEvent(string type, string soundResource, int sourceEntityID)
            : base(type)
        {
            this.SoundResource = soundResource;
            this.SourceEntityID = sourceEntityID;
        }

        public override string ToString()
        {
            return base.ToString() + " [" + SoundResource + " is being issued to " + Type + " by SourceEntityID " 
                + SourceEntityID + "]";
        }
    }
}
