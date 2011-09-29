using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Audio
{
    public interface IAudioPlayer
    {
        void PlaySound(string resourceID);
    }
}
