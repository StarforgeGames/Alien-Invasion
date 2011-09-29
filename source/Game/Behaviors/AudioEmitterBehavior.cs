using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement.Events;
using Game.Entities;

namespace Game.Behaviors
{
	public class AudioEmitterBehavior : AEntityBasedBehavior
	{
		public const string Key_CreateEntitySound = "CreateEntitySound";
		public const string Key_DestroyEntitySound = "DestroyEntitySound";

		List<AudioEvent> audioQueue = new List<AudioEvent>();

		public AudioEmitterBehavior(Entity entity)
			: base(entity)
		{
			entity.AddAttribute(Key_CreateEntitySound, new Attribute<string>(String.Empty));
			entity.AddAttribute(Key_DestroyEntitySound, new Attribute<string>(String.Empty));

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
				eventManager.QueueEvent(audioEvent);
			}
			audioQueue.Clear();
		}

		public override void OnEvent(Event evt)
		{
			AudioEvent audioEvent = null;

			switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY: {
                    NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                    if (entity.ID != newEntityEvent.EntityID) {
                        break;
                    }

					Attribute<string> sound = entity[Key_CreateEntitySound] as Attribute<string>;
					if (String.IsNullOrEmpty(sound.Value)) {
						break;
					}

					audioEvent = new AudioEvent(AudioEvent.PLAY_SOUND, sound.Value, entity.ID);
					break;
				}
                case DestroyEntityEvent.DESTROY_ENTITY: {
                    DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
                    if (entity.ID != destroyEntityEvent.EntityID) {
                        break;
                    }

					Attribute<string> sound = entity[Key_DestroyEntitySound] as Attribute<string>;
					if (String.IsNullOrEmpty(sound.Value)) {
						break;
					}

					audioEvent = new AudioEvent(AudioEvent.PLAY_SOUND, sound.Value, entity.ID);
					break;
				}
			}

			if (audioEvent != null) {
				audioQueue.Add(audioEvent);
			}
		}
	}
}
