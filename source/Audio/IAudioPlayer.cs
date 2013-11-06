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

		void PlayEffect(ResourceHandle handle, SoundGroup group = SoundGroup.InGameEffect);

		void CreateLoopingSound(SoundGroup group, string file, bool paused = false);
		void CreateLoopingSound(SoundGroup group, ResourceHandle handle, bool paused = false);

		void StopLoopingSounds(SoundGroup group);
		void PauseLoopingSounds(SoundGroup group);
		void UnpauseLoopingSounds(SoundGroup group);

		Sound CreateSoundFrom(byte[] data);
		Sound CreateStreamFrom(byte[] data);
	}
}
