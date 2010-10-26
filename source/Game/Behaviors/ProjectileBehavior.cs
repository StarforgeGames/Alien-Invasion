using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{

    class ProjectileBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_ProjectileOwner = "ProjectileOwner";

        public ProjectileBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_ProjectileOwner, new Attribute<Entity>(null));
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<Rectangle> bounds = entity[SpatialBehavior.Key_Bounds] as Attribute<Rectangle>;

            if (bounds.Value.Top <= 0 || bounds.Value.Bottom >= entity.Game.WorldHeight) {
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
            EventManager.QueueEvent(new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID));
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
