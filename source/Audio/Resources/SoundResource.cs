using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Resources;

namespace Audio.Resources
{
    public class SoundResource : AResource
    {
        public FMOD.Sound Sound { get; private set; }

        internal SoundResource(FMOD.Sound sound)
        {
            Sound = sound;
        }
        
    }
}
