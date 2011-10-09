using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{

    public class ProjectileBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_ProjectileOwner = "ProjectileOwner";

        public ProjectileBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_ProjectileOwner, -1);

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        {
            Vector2D atBoundary = entity[SpatialBehavior.Key_AtBoundary];

            if (atBoundary.X != 0.0f || atBoundary.Y != 0.0f) {
                killEntity();
                Console.WriteLine("[" + this.GetType().Name +"] " + entity.Type + " died in vain.");
            }
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(CollisionEvent));
        }

        private void killEntity()
        {
            entity.State = EntityState.Dead;
            eventManager.QueueEvent(DestroyEntityEvent.Destroy(entity.ID));
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case CollisionEvent.ACTOR_COLLIDES: {
                    killEntity();

                    Console.WriteLine("[" + this.GetType().Name +"] " + entity.Type + " died fulfilling its honorable "
                        + "duty.");
                }
                break;
            }
        }
    }

}
