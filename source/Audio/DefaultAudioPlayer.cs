using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using ResourceManagement;
using Audio.Resources;
using Utility.Threading;
using System.Threading;
using System.Runtime.InteropServices;

namespace Audio
{
    public class DefaultAudioPlayer : IAudioPlayer, IDisposable
    {
        public IAsyncExecutor Queue
        {
            get { return queue; }
        }
        private CommandQueue queue = new CommandQueue();

        private FMOD.System system;

        private const int ChannelCount = 32;
        private Channel[] channels;
        private Channel loopChannel;
        private int currentChannel;

        private Thread audioThread;
        private bool isRunning = false;

        public DefaultAudioPlayer()
        {
            RESULT result = Factory.System_Create(ref system);
            if (result != RESULT.OK)
            {
                // TODO: Check result, error handling. Show Popup message or something the like
            }
            system.init(ChannelCount, INITFLAGS.NORMAL, (IntPtr)null);

            channels = new Channel[ChannelCount];
            loopChannel = channels[ChannelCount - 1];

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
                        system.playSound(CHANNELINDEX.REUSE, resource.Sound, false, ref channels[currentChannel]);
                    }
                    currentChannel = (currentChannel + 1) % (ChannelCount - 1);
                });
        }

        public void StartLoopingSound(ResourceHandle handle)
        {
            Queue.Add(() => {
                   using (var resource = (SoundResource)handle.Acquire())
                   {
                       resource.Sound.setMode(MODE.LOOP_NORMAL);
                       system.playSound(FMOD.CHANNELINDEX.REUSE, resource.Sound, false, ref loopChannel);
                   }
               });
        }

        public void StopLoopingSound()
        {
            if (loopChannel == null)
            {
                return;
            }

            loopChannel.stop();
        }

        public void PauseLoopingSounds()
        {
            if (loopChannel == null)
            {
                return;
            }

            loopChannel.setPaused(true);
        }

        public void UnpauseLoopingSounds()
        {
            if (loopChannel == null)
            {
                return;
            }

            loopChannel.setPaused(false);
        }

        public Sound CreateSoundFrom(byte[] data)
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
    }
}
