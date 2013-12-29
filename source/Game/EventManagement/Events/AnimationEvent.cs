using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{
	class AnimationEvent : Event
	{
		public const string PLAY_ANIMATION = "PLAY_ANIMATION";
		public const string PAUSE_ANIMATION = "PAUSE_ANIMATION";
		public const string STOP_ANIMATION = "STOP_ANIMATION";
		public const string ANIMATION_STOPPED = "ANIMATION_STOPPED";

		public bool Loops { get; private set; }

		public bool ResetOnStop { get; set; }

		private AnimationEvent(int recipientId, string type, bool loop)
			: base(type, recipientId)
		{
			Loops = loop;
			ResetOnStop = true;
		}

		private AnimationEvent(string type, bool loop)
			: base(type)
		{
			Loops = loop;
			ResetOnStop = true;
		}

		public static AnimationEvent Play(int recipientId)
		{
			return new AnimationEvent(recipientId, PLAY_ANIMATION, false);
		}

		public static AnimationEvent PlayLoop(int recipientId)
		{
			return new AnimationEvent(recipientId, PLAY_ANIMATION, true);
		}

		public static AnimationEvent Pause(int recipientId)
		{
			return new AnimationEvent(recipientId, PAUSE_ANIMATION, false);
		}

		public static AnimationEvent Stop(int recipientId)
		{
			return new AnimationEvent(recipientId, STOP_ANIMATION, false);
		}

		public static AnimationEvent Stopped(int recipientId)
		{
			return new AnimationEvent(recipientId, ANIMATION_STOPPED, false);
		}

		public static AnimationEvent Stopped()
		{
			return new AnimationEvent(ANIMATION_STOPPED, false);
		}
	}
}
