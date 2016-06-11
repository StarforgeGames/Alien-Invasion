using Game.Entities;
using Game.EventManagement.Events;
using ResourceManagement;

namespace Game.Behaviors
{
    public class DyingBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_DeathAnimation = "DeathAnimation";

        public DyingBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_DeathAnimation, (ResourceHandle)null);

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(AnimationEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (entity.State != EntityState.Dying)
            {
                return;
            }

            var evt = AnimationEvent.Play(entity.ID); // TODO: Start Death Animation explicitly, not just any animation
            evt.ResetOnStop = false;
            eventManager.Queue(evt);
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type)
            {
                case AnimationEvent.ANIMATION_STOPPED:
                {
                    AnimationEvent aniMsg = (AnimationEvent)evt;
                    entity.State = EntityState.Dead;
                    entity[RenderBehavior.Key_IsRenderable] = false;
                    break;
                }
            }
        }
    }
}
