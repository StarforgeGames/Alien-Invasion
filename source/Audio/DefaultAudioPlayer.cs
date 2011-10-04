using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using ResourceManagement;
using Audio.Resources;
using System.Threading;
using System.Runtime.InteropServices;

namespace Audio
{
    public class DefaultAudioPlayer : IAudioPlayer, IDisposable
    {
        private FMOD.System system;
        private FMOD.Channel[] channels;
        private int currentChannel = 0;

        private Thread audioThread;

        private BlockingCommandQueue queue = new BlockingCommandQueue();

        private const int ChannelCount = 32;
        private bool isRunning = false;

        public DefaultAudioPlayer()
        {
            RESULT result = FMOD.Factory.System_Create(ref system);
            system.init(ChannelCount, FMOD.INITFLAGS.NORMAL, (IntPtr)null);
            channels = new Channel[ChannelCount];

            audioThread = new Thread(audioLoop);
            audioThread.Name = "Audio";
        }

        public void Start()
        {
            isRunning = true;
            audioThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
            queue.Cancel();

        }

        private void audioLoop()
        {
            while (isRunning)
            {
                queue.Execute();
            }
        }


        public void PlaySound(ResourceHandle handle)
        {
            Queue.Add(() =>
                {
                    using (var resource = (SoundResource)handle.Acquire())
                    {
                        system.playSound(FMOD.CHANNELINDEX.REUSE, resource.Sound, false, ref channels[currentChannel]);
                    }
                    currentChannel = (currentChannel + 1) % ChannelCount;
                });
        }

        internal Sound CreateSoundFrom(byte[] data)
        {
            Sound sound = null;
            CREATESOUNDEXINFO info = new CREATESOUNDEXINFO();
            info.cbsize = Marshal.SizeOf(info);
            info.length = (uint)data.Length;
            //info.format = SOUND_FORMAT.MPEG;

            RESULT res = system.createSound(data, MODE.HARDWARE | MODE.OPENMEMORY, ref info, ref sound); // hier läufts schief, muss ich mal schaun
            return sound;
        }

        public void Dispose()
        {
            Stop();
            audioThread.Join();
            FMOD.RESULT result;

            if (system != null) {
                result = system.close();
                result = system.release();
            }
        }

        public IAsyncExecutor Queue 
        {
            get
            {
                return queue;
            }
        }
    }
}
