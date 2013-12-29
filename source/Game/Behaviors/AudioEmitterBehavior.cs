using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement.Events;
using Game.Entities;
using ResourceManagement;

namespace Game.Behaviors
{
	public class AudioEmitterBehavior : AEntityBasedBehavior
	{
		public const string Key_SoundEffects = "SoundEffects";

		private List<AudioEvent> audioQueue = new List<AudioEvent>();

		public AudioEmitterBehavior(Entity entity)
			: base(entity)
		{
			entity.AddAttribute(Key_SoundEffects, (AudioInfo)null);

			initializeHandledEventTypes();
		}

		protected override void initializeHandledEventTypes()
		{
			handledEventTypes.Add(typeof(NewEntityEvent));
			handledEventTypes.Add(typeof(DestroyEntityEvent));
		}

		public override void OnUpdate(float deltaTime)
		{
			foreach (var audioEvent in audioQueue) {
				eventManager.Queue(audioEvent);
			}
			audioQueue.Clear();
		}

		public override void OnEvent(Event evt)
		{
			AudioEvent audioEvent = null;
			AudioInfo sounds = entity[Key_SoundEffects];

			if (sounds == null)
			{
				throw new Exception(String.Format("No sound effects defined for Entity '{0}'", entity.ToString()));
			}
			AudioInfo.Sound sound = sounds.GetSoundForEvent(evt.Type);
			if (sound == null)
			{
				return;
			}

			switch (evt.Type)
			{
				case CreateEntityEvent.CREATE_ENTITY:
					CreateEntityEvent createEntityEvent = (CreateEntityEvent)evt;
					break;
				case DestroyEntityEvent.DESTROY_ENTITY:
					DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
					if(entity.ID != destroyEntityEvent.EntityID)
					{
						return;
					}
					break;
			}

			audioEvent = AudioEvent.PlaySound(entity.ID, sound.EventName, sound.Project);

			if (audioEvent != null) {
				audioQueue.Add(audioEvent);
			}
		}
	}
}
