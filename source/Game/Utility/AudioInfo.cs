using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Behaviors
{
	public class AudioInfo
	{
		public class Sound 
		{
			public string EventName { get; set; }
			public string Project { get; set; }
		}

		private Dictionary<string, Sound> eventToSoundMap = new Dictionary<string, Sound>();

		public void Add(string onGameEvent, Sound sound)
		{
			if (eventToSoundMap.ContainsKey(onGameEvent))
			{
				eventToSoundMap[onGameEvent] = sound;
			}
			else
			{
				eventToSoundMap.Add(onGameEvent, sound);
			}
		}

		public void Remove(string onGameEvent)
		{
			if (eventToSoundMap.ContainsKey(onGameEvent))
			{
				eventToSoundMap.Remove(onGameEvent);
			}
		}

		public Sound GetSoundForEvent(string gameEvent)
		{
			if (!eventToSoundMap.ContainsKey(gameEvent))
			{
				return null;
			}

			return eventToSoundMap[gameEvent];
		}
	}
}
