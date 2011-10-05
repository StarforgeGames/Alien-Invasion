using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;

namespace Game.EventManagement.Events
{
    public class AudioEvent : Event
    {
         // Event Message Types
        public const string PLAY_SOUND = "play_sound";
        public const string STOP_SOUND = "stop_sound";

        public ResourceHandle SoundResource { get; private set; }
        public int EntityID { get; set; }

        private AudioEvent(string type, int entityID, ResourceHandle soundResource)
            : base(type)
        {
            this.EntityID = entityID;
            this.SoundResource = soundResource;
        }

        public override string ToString()
        {
            return base.ToString() + " [" + SoundResource + " is being issued to " + Type + " by SourceEntityID " 
                + EntityID + "]";
        }

        public static AudioEvent PlaySound(int entityID, ResourceHandle soundResource)
        {
            return new AudioEvent(PLAY_SOUND, entityID, soundResource);
        }

        public static AudioEvent StopSound(int entityID, ResourceHandle soundResource)
        {
            return new AudioEvent(STOP_SOUND, entityID, soundResource);
        }
    }
}
