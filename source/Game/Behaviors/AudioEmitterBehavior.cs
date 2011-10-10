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
        public const string Key_CreateEntitySound = "CreateEntitySound";
        public const string Key_DestroyEntitySound = "DestroyEntitySound";

        List<AudioEvent> audioQueue = new List<AudioEvent>();

        public AudioEmitterBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_CreateEntitySound, (ResourceHandle)null);
            entity.AddAttribute(Key_DestroyEntitySound, (ResourceHandle)null);

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

            switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY: {
                    NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                    if (entity.ID != newEntityEvent.EntityID) {
                        break;
                    }

                    ResourceHandle sound = entity[Key_CreateEntitySound];
                    if (sound == null) {
                        break;
                    }
                    
                    if (entity.Type == "mystery_ship")
                    {
                        audioEvent = AudioEvent.LoopSound(entity.ID, sound);
                    }
                    else
                    {
                        audioEvent = AudioEvent.PlaySound(entity.ID, sound);
                    }
                    break;
                }
                case DestroyEntityEvent.DESTROY_ENTITY: {
                    DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
                    if (entity.ID != destroyEntityEvent.EntityID) {
                        break;
                    }
                    
                    // No explosion sound when mystery ship is not killed by player!
                    if (entity.Type == "mystery_ship" && destroyEntityEvent.DestroyedByEntityID == 0)
                    {
                        break;
                    }

                    ResourceHandle sound = entity[Key_DestroyEntitySound];
                    if (sound == null) {
                        break;
                    }

                    audioEvent = AudioEvent.PlaySound(entity.ID, sound);
                    break;
                }
            }

            if (audioEvent != null) {
                using (audioEvent.SoundResource.Acquire()) { }// Workaround: Acquire right away so sound is played first time
                audioQueue.Add(audioEvent);
            }
        }
    }
}
