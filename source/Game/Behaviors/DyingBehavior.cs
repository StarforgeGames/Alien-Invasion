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

            Attribute<bool> hasDeathAnimation = entity[Key_HasDeathAnimation];
            if (hasDeathAnimation)
            {
                Attribute<ResourceHandle> deathAnimation = entity[Key_DeathAnimation];
                // TODO: Start Death Animation explicitly, not just any animation
                var evt = AnimationEvent.Play(entity.ID);
                evt.ResetOnStop = false;
                eventManager.QueueEvent(evt);
            }
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
