using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;

namespace Audio
{
    public interface IAudioPlayer
    {
        void PlaySound(ResourceHandle handle);
        void StartLoopingSound(ResourceHandle handle);
        void StopLoopingSound();
    }
}
