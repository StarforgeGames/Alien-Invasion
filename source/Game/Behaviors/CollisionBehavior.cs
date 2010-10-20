using System;
using System.Collections.Generic;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    class CollisionBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_IsPhysical = "IsPhysical";
        public const string Key_CollisionDamage = "CollisionDamage";

        public CollisionBehavior(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { };

            entity.AddAttribute(Key_IsPhysical, new Attribute<bool>(true));
            entity.AddAttribute(Key_CollisionDamage, new Attribute<int>(1));
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (Entity other in entity.Game.Entities.Values) {
                if (entity.ID == other.ID || other.State != EntityState.Active) {
                    continue;
                }

                Attribute<bool> isPhysical = other[Key_IsPhysical] as Attribute<bool>;
                if (isPhysical == null || !isPhysical) {
                    continue;
                }

                Attribute<Entity> owner = entity[ProjectileBehavior.Key_ProjectileOwner] as Attribute<Entity>;
                Attribute<Entity> otherOwner = other[ProjectileBehavior.Key_ProjectileOwner] as Attribute<Entity>;
                if ((owner != null && owner.Value == other) || (otherOwner != null && otherOwner.Value == entity)) {
                    continue;
                }

                Attribute<Rectangle> otherBounds = other[SpatialBehavior.Key_Bounds] as Attribute<Rectangle>;
                if (isColliding(otherBounds)) {
                    Console.WriteLine("[" + this.GetType().Name + "] " + entity.Type + " collided with " + other.Type
                        + "!");

                    CollisionEvent collisionMsg = new CollisionEvent(
                        CollisionEvent.ACTOR_COLLIDES, 
                        other.ID, 
                        entity.ID);
                    EventManager.QueueEvent(collisionMsg);

                    Attribute<int> collisionDmg = entity[Key_CollisionDamage] as Attribute<int>;
                    DamageEvent dmgMsg = new DamageEvent(DamageEvent.RECEIVE_DAMAGE,
                        other.ID,
                        collisionDmg, 
                        entity.ID);
                    EventManager.QueueEvent(dmgMsg);
                }
            }
        }

        public override void OnEvent(Event evt)
        {
        }

        private bool isColliding(Attribute<Rectangle> other)
        {
            Attribute<Rectangle> bounds = entity[SpatialBehavior.Key_Bounds] as Attribute<Rectangle>;

            return bounds.Value.Left <= other.Value.Right
                && bounds.Value.Top <= other.Value.Bottom
                && bounds.Value.Right >= other.Value.Left
                && bounds.Value.Bottom >= other.Value.Top;
        }
    }
}
