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
	public enum SoundGroup
	{
		Menu,
		InGameEffect,
		InGameMusic
	}

	public class DefaultAudioPlayer : IAudioPlayer, IDisposable
	{
		public IAsyncExecutor Queue
		{
			get { return queue; }
		}
		private CommandQueue queue = new CommandQueue();

		private FMOD.System system;
		
		private const int ChannelCount = 32;

		private ChannelGroup groupMenuMusic;
		private ChannelGroup groupInGameEffects;
		private ChannelGroup groupInGameMusic;

		private Dictionary<SoundGroup, ChannelGroup> groupMap = new Dictionary<SoundGroup, ChannelGroup>();

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
			
			system.createChannelGroup("menu_music", ref groupMenuMusic);
			system.createChannelGroup("ingame_effects", ref groupInGameEffects);
			system.createChannelGroup("ingame_music", ref groupInGameMusic);

			groupInGameMusic.setVolume(0.7f);

			groupMap[SoundGroup.Menu] = groupMenuMusic;
			groupMap[SoundGroup.InGameEffect] = groupInGameEffects;
			groupMap[SoundGroup.InGameMusic] = groupInGameMusic;

			audioThread = new Thread(audioLoop);
			audioThread.Name = "Audio";
		}

		public void Start()
		{
			if (isRunning)
			{
				return;
			}

			isRunning = true;
			audioThread.Start();
		}

		public void Stop()
		{
			if (!isRunning)
			{
				return;
			}

			isRunning = false;
			queue.Cancel();
			audioThread.Join();
		}

		private void audioLoop()
		{
			while (isRunning)
			{
				queue.Execute();
			}
		}


		public void Play(Sound sound, float volume = 1.0f, SoundGroup group = SoundGroup.InGameEffect)
		{
			Queue.Add(() =>
				{
					Channel channel = null;
					system.playSound(CHANNELINDEX.FREE, sound, true, ref channel);
						
					channel.setChannelGroup(groupMap[group]);
					channel.setVolume(volume);
					channel.setPaused(false);
				});
		}

		public void CreateLoopingSound(SoundGroup group, Sound sound, bool paused = false, float volume = 1.0f)
		{
			Queue.Add(() => {
					Channel channel = null;
					system.playSound(FMOD.CHANNELINDEX.FREE, sound, true, ref channel);
					   
					channel.setChannelGroup(groupMap[group]);
					channel.setMode(MODE.LOOP_NORMAL);
					channel.setLoopCount(-1);
					channel.setVolume(volume);

					channel.setPaused(paused);
			   });
		}

		public void CreateLoopingSound(SoundGroup group, string file, bool paused = false, float volume = 1.0f)
		{
			Queue.Add(() =>
			{
				Sound sound = null;
				system.createStream(file, MODE.CREATESTREAM, ref sound);

				Channel channel = null;
				system.playSound(FMOD.CHANNELINDEX.FREE, sound, true, ref channel);

				channel.setChannelGroup(groupMap[group]);
				channel.setMode(MODE.LOOP_NORMAL);
				channel.setLoopCount(-1);
				channel.setVolume(volume);

				channel.setPaused(paused);
			});
		}

		public void StopLoopingSounds(SoundGroup group)
		{
			Queue.Add(() =>
			{
				ChannelGroup cGroup = groupMap[group];
				int numOfChannels = 0;
				cGroup.getNumChannels(ref numOfChannels);

				for (int i = 0; i < numOfChannels; i++)
				{
					Channel channel = null;
					cGroup.getChannel(i, ref channel);
					channel.stop();
				}
			});
		}

		public void PauseLoopingSounds(SoundGroup group)
		{
			setLoopingSoundPause(group, true);
		}

		public void UnpauseLoopingSounds(SoundGroup group)
		{
			setLoopingSoundPause(group, false);
		}

		private void setLoopingSoundPause(SoundGroup group, bool isPaused)
		{
			Queue.Add(() =>
			{
				ChannelGroup cGroup = groupMap[group];
				int numOfChannels = 0;
				cGroup.getNumChannels(ref numOfChannels);

				for (int i = 0; i < numOfChannels; i++)
				{
					Channel channel = null;
					cGroup.getChannel(i, ref channel);
					channel.setPaused(isPaused);
				}
			});
		}

		public Sound CreateSoundFrom(byte[] data)
		{
			CREATESOUNDEXINFO info = new CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
			info.length = (uint)data.Length;

			Sound sound = null;
			RESULT res = system.createSound(data, (MODE.HARDWARE | MODE.OPENMEMORY), ref info, ref sound);

			return sound;
		}

		public Sound CreateStreamFrom(byte[] data)
		{
			CREATESOUNDEXINFO info = new CREATESOUNDEXINFO();
			info.cbsize = Marshal.SizeOf(info);
			info.length = (uint)data.Length;

			Sound sound = null;
			RESULT res = system.createStream(data, (MODE.CREATESTREAM | MODE.HARDWARE | MODE.OPENMEMORY), ref info, ref sound);
			
			return sound;
		}

		public void Dispose()
		{
			Stop();

			FMOD.RESULT result;
			if (system != null) {
				result = system.close();
				result = system.release();
			}
		}
	}
}
