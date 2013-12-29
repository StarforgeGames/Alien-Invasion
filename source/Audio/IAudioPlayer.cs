using System;
using FMOD;
using ResourceManagement;
using Utility.Threading;

namespace Audio
{
	public interface IAudioPlayer : IDisposable
	{
		bool IsPaused { get; set; }

		void LoadGroup(string group);
		void LoadFile(string file);

		void OnUpdate();

		void PlayEvent(string soundEvent, float volume = 1.0f);
		void StopEvent(string soundEvent);
		void PauseEvent(string soundEvent);
		void UnpauseEvent(string soundEvent);
		bool IsEventPaused(string soundEvent);

		void PauseCategory(string category);
		void UnpauseCategory(string category);
		void StopCategory(string category);
	}
}
