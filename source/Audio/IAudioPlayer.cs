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

        void PlaySound(ResourceHandle handle);

        void StartLoopingSound(ResourceHandle handle);
        void StopLoopingSound();

        void PauseLoopingSounds();
        void UnpauseLoopingSounds();

        Sound CreateSoundFrom(byte[] data);
    }
}
