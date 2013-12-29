using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using ResourceManagement;
using Utility.Threading;
using System.Threading;
using System.Runtime.InteropServices;

namespace Audio
{
	public class FmodAudioPlayer : IAudioPlayer, IDisposable
	{
		public bool IsPaused { get; set; }

		private const int ChannelCount = 128;

		private EventSystem eventSystem = null;

		//private Dictionary<string, FMOD.Event> soundEventMap = new Dictionary<string, Event>();

		public FmodAudioPlayer(string mediaPath)
		{
			RESULT result = FMOD.Event_Factory.EventSystem_Create(ref eventSystem);
			fmodErrorCheck(result);
			result = eventSystem.init(ChannelCount, FMOD.INITFLAGS.NORMAL, (IntPtr)null, FMOD.EVENT_INITFLAGS.NORMAL);
			fmodErrorCheck(result);
			result = eventSystem.setMediaPath(mediaPath);
			fmodErrorCheck(result);
		}

		private void fmodErrorCheck(RESULT result)
		{
			if (result != RESULT.OK)
			{
				Console.WriteLine(FMOD.Error.String(result));
				Console.Error.WriteLine(FMOD.Error.String(result));
				throw new Exception(FMOD.Error.String(result));
			}
		}

		public void LoadFile(string file)
		{
			RESULT result = eventSystem.load(file);
			fmodErrorCheck(result);
		}

		public void PreloadGroup(string name)
		{
			EventGroup group = null;
			RESULT result = eventSystem.getGroup(name, true, ref group);
			fmodErrorCheck(result);

			result = group.loadEventData();
			fmodErrorCheck(result);
		}

		public void OnUpdate()
		{
			if (!IsPaused)
			{
				RESULT result = eventSystem.update();
				fmodErrorCheck(result);
			}
		}

		public void PlayEvent(string soundEvent, float volume = 1.0f)
		{
			Event evt = getEvent(soundEvent);

			RESULT result = evt.start();
			fmodErrorCheck(result);
		}

		private Event getEvent(string eventName)
		{
			Event evt = null;
			RESULT result = eventSystem.getEvent(eventName, FMOD.EVENT_MODE.DEFAULT, ref evt);
			fmodErrorCheck(result);

			return evt;
		}

		public void StopEvent(string soundEvent)
		{
			Event evt = getEvent(soundEvent);

			RESULT result = evt.stop();
		}

		public void PauseEvent(string soundEvent)
		{
			Event evt = getEvent(soundEvent);

			RESULT result = evt.setPaused(true);
			fmodErrorCheck(result);
		}

		public void UnpauseEvent(string soundEvent)
		{
			Event evt = getEvent(soundEvent);

			RESULT result = evt.setPaused(false);
			fmodErrorCheck(result);
		}

		public bool IsEventPaused(string soundEvent)
		{
			Event evt = getEvent(soundEvent);

			bool isPaused = false;
			RESULT result = evt.getPaused(ref isPaused);
			fmodErrorCheck(result);

			return isPaused;
		}

		public void StopCategory(string name)
		{
			EventCategory category = getEventCategory(name);
			RESULT result = category.stopAllEvents();
			fmodErrorCheck(result);
		}

		public void PauseCategory(string name)
		{
			EventCategory category = getEventCategory(name);
			RESULT result = category.setPaused(true);
			fmodErrorCheck(result);
		}

		public void UnpauseCategory(string name)
		{
			EventCategory category = getEventCategory(name);
			RESULT result = category.setPaused(false);
			fmodErrorCheck(result);
		}

		public bool IsCategoryPaused(string name)
		{
			EventCategory category = getEventCategory(name);

			bool isPaused = false;
			RESULT result = category.getPaused(ref isPaused);
			fmodErrorCheck(result);

			return isPaused;
		}

		private EventCategory getEventCategory(string name)
		{
			EventCategory category = null;
			RESULT result = eventSystem.getCategory(name, ref category);
			fmodErrorCheck(result);

			return category;
		}
		
		public void Dispose()
		{
			IsPaused = true;

			FMOD.RESULT result;
			if (eventSystem != null) {
				result = eventSystem.unload();
				fmodErrorCheck(result);
				result = eventSystem.release();
				fmodErrorCheck(result);
			}
		}
	}
}
