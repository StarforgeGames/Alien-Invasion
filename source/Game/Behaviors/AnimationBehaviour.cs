using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{
    class AnimationBehaviour : AEntityBasedBehavior
    {
        public const string Key_Frame = "Frame";
        public const string Key_FrameCount = "FrameCount"; // this has to be removed in a future version!

        private bool isPlaying;
        private float elapsedTime;

        AnimationBehaviour(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Frame, new Attribute<int>(0));
            entity.AddAttribute(Key_FrameCount, new Attribute<int>(0));

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (isPlaying)
            {
                elapsedTime += deltaTime;
                if (elapsedTime > 40.0f)
                {
                    elapsedTime = 0;
                    int frame = entity[Key_Frame];
                    // we check for frameCount since the last frame should also be visible for some time
                    if (frame < entity[Key_FrameCount])
                    {
                        entity[Key_Frame] = frame + 1;
                    }
                    else
                    {
                        isPlaying = false;
                        eventManager.QueueEvent(AnimationEvent.Stopped(entity.ID));
                    }
                }
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type)
            {
                case AnimationEvent.PLAY_ANIMATION:
                    isPlaying = true;
                    break;
                case AnimationEvent.STOP_ANIMATION:
                    isPlaying = false;
                    entity[Key_Frame] = 0;
                    eventManager.QueueEvent(AnimationEvent.Stopped(entity.ID));
                    break;
                case AnimationEvent.PAUSE_ANIMATION:
                    isPlaying = false;
                    entity[Key_Frame] = 0;
                    eventManager.QueueEvent(AnimationEvent.Stopped(entity.ID));
                    break;
                default:
                    break;
            }
        }
    }
}
