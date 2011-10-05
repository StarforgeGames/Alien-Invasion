using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.EventManagement.Events;
using ResourceManagement;

namespace Game.Behaviors
{
    public class DyingBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_HasDeathAnimation = "HasDeathAnimation";
        public const string Key_DeathAnimation = "DeathAnimation";

        public DyingBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_HasDeathAnimation, new Attribute<bool>(true));
            entity.AddAttribute(Key_DeathAnimation, new Attribute<ResourceHandle>(null));

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

            // TODO: Start Death Animation explicitly, not just any animation
            eventManager.QueueEvent(AnimationEvent.Play(entity.ID));
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type)
            {
                case AnimationEvent.ANIMATION_STOPPED:
                {
                    AnimationEvent aniMsg = (AnimationEvent)evt;
                    this.entity.State = EntityState.Dead;
                    break;
                }
            }
        }
    }
}
