using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace Audio
{
    public class DefaultAudioPlayer : IAudioPlayer, IDisposable
    {
        private FMOD.System system;
        private FMOD.Channel channel1;
        private FMOD.Channel channel2;
        private FMOD.Channel channel3;

        // TO-DO: Use ResourceManager for Sounds
        private FMOD.Sound alienShot;
        private FMOD.Sound playerShot;
        private FMOD.Sound explosion;
        
        private bool isPlaying = false;

        public DefaultAudioPlayer()
        {
            initialize();
        }

        private void initialize() 
        {
            RESULT result = FMOD.Factory.System_Create(ref system);
            system.init(32, FMOD.INITFLAGS.NORMAL, (IntPtr)null);

            // TO-DO: Use ResourceManager for Sounds
            system.createSound(@"data\audio\alien_shot.mp3", FMOD.MODE.HARDWARE, ref alienShot);
            system.createSound(@"data\audio\player_shot.mp3", FMOD.MODE.HARDWARE, ref playerShot);
            system.createSound(@"data\audio\explosion.mp3", FMOD.MODE.HARDWARE, ref explosion);
        }

        public void PlaySound(string resourceID)
        {                    
            switch (resourceID) {
                case @"data\audio\alien_shot.mp3": {
                    system.playSound(FMOD.CHANNELINDEX.REUSE, alienShot, false, ref channel1);
                        break;
                    }
                case @"data\audio\player_shot.mp3": {
                    system.playSound(FMOD.CHANNELINDEX.REUSE, playerShot, false, ref channel2);
                        break;
                    }
                case @"data\audio\explosion.mp3": {
                    system.playSound(FMOD.CHANNELINDEX.REUSE, explosion, false, ref channel3);
                    break;
                }
            }
        }

        public void Dispose()
        {
            FMOD.RESULT result;

            if (alienShot != null) {
                result = alienShot.release();
            }
            if (playerShot != null) {
                result = playerShot.release();
            }
            if (explosion != null) {
                result = explosion.release();
            }

            if (system != null) {
                result = system.close();
                result = system.release();
            }
        }
    }
}
