using System;
using FMOD;
using ResourceManagement;
using Utility.Threading;

namespace Audio
{
	public interface IAudioPlayer : IDisposable
	{
		IAsyncExecutor Queue { get; }

		void Start();
		void Stop();

		void Play(Sound sound, float volume = 1.0f, SoundGroup group = SoundGroup.InGameEffect);

		void CreateLoopingSound(SoundGroup group, string file, bool paused = false, float volume = 1.0f);
		void CreateLoopingSound(SoundGroup group, Sound sound, bool paused = false, float volume = 1.0f);

		void StopLoopingSounds(SoundGroup group);
		void PauseLoopingSounds(SoundGroup group);
		void UnpauseLoopingSounds(SoundGroup group);

		Sound CreateSoundFrom(byte[] data);
		Sound CreateStreamFrom(byte[] data);
	}
}
