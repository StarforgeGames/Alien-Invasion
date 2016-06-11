using System;

namespace Game.EventManagement.Events
{
	public class AudioEvent : Event
	{
		 // Event Message Types
		public const string PLAY_SOUND = "PLAY_SOUND";
		public const string PAUSE_SOUND = "PAUSE_SOUND";
		public const string STOP_SOUND = "STOP_SOUND";

		public int EntityID { get; private set; }

		public string Project { get; private set; }
		public string SoundEvent { get; private set; }

		public bool Loop { get; private set; }
		public float Volume { get; private set; }

		private AudioEvent(string type, int entityID, string soundEvent, string project, bool loop = false, 
			float volume = 1.0f)
			: base(type)
		{
			this.EntityID = entityID;

			this.Project = project;
			this.SoundEvent = soundEvent;

			this.Loop = loop;
			this.Volume = volume;
		}

		public static AudioEvent PlaySound(int entityID, string soundEvent, string project,	float volume = 1.0f)
		{
			return new AudioEvent(PLAY_SOUND, entityID, soundEvent, project, false, volume);
		}

		public static AudioEvent LoopSound(int entityID, string soundEvent, string project, float volume = 1.0f)
		{
			return new AudioEvent(PLAY_SOUND, entityID, soundEvent, project, true, volume);
		}

		public static AudioEvent StopSound(int entityID, string soundEvent, string project)
		{
			return new AudioEvent(STOP_SOUND, entityID, soundEvent, project);
		}

		public override string ToString()
		{
			return String.Format("audio/{0}/{1}", Project, SoundEvent);
		}
	}
}
