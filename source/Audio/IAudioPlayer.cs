using System;
using FMOD;
using ResourceManagement;
using Utility.Threading;

namespace Audio
{
	public interface IAudioPlayer : IDisposable
	{
		bool IsPaused { get; set; }

		void LoadFile(string file);
		void PreloadGroup(string group);

		void OnUpdate();

		void PlayEvent(string soundEvent, float volume = 1.0f);
		void StopEvent(string soundEvent);
		void PauseEvent(string soundEvent);
		void UnpauseEvent(string soundEvent);
		bool IsEventPaused(string soundEvent);

		void StopCategory(string name);
		void PauseCategory(string name);
		void UnpauseCategory(string name);
		bool IsCategoryPaused(string name);
	}
}
