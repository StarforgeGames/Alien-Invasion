using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{
    public class AnimationBehavior : AEntityBasedBehavior
    {
        public const string Key_Frame = "Frame";
        public const string Key_FrameCount = "FrameCount"; // this has to be removed in a future version!
        public const string Key_FrameRate = "FrameRate"; // this has to be removed in a future version!

        private bool isPlaying = false;
        private float elapsedTime;
        private bool resetOnStopped = true;
        private bool loop = false;

        public AnimationBehavior(Entity entity)
            : base(entity)
        {
            //entity.AddAttribute(Key_Frame, new Attribute<float>(0.0f)); // this variable is owned by render behaviour
            entity.AddAttribute(Key_FrameCount, 1.0f);
            entity.AddAttribute(Key_FrameRate, 30.0f);

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(AnimationEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!isPlaying)
            {
                return;
            }

            elapsedTime += deltaTime;
            if (elapsedTime > (1.0f / entity[Key_FrameRate]))
            {
                elapsedTime = 0.0f;
                float frame = entity[Key_Frame];

                if (frame < entity[Key_FrameCount] - 1.0f)
                {
                    entity[Key_Frame] = frame + 1.0f;
                }
                else
                {
                    if (!loop)
                    {
                        isPlaying = false;
                        eventManager.Queue(AnimationEvent.Stopped(entity.ID));
                    }
                    if (resetOnStopped || loop)
                    {
                        entity[Key_Frame] = 0.0f;
                    }
                }
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type)
            {
                case AnimationEvent.PLAY_ANIMATION:
                    var animationEvent = (AnimationEvent)evt;
                    resetOnStopped = animationEvent.ResetOnStop;
                    loop = animationEvent.Loops;
                    isPlaying = true;
                    break;
                case AnimationEvent.STOP_ANIMATION:
                    isPlaying = false;
                    entity[Key_Frame] = 0.0f;
                    eventManager.Queue(AnimationEvent.Stopped(entity.ID));
                    break;
                case AnimationEvent.PAUSE_ANIMATION:
                    isPlaying = false;
                    entity[Key_Frame] = 0.0f;
                    eventManager.Queue(AnimationEvent.Stopped(entity.ID));
                    break;
                default:
                    break;
            }
        }
    }
}
