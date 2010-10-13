using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.Utility;
using Game.EventManagement.Events;

namespace Game.Behaviours
{
    class CollisionBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_IsPhysical = "IsPhysical";
        public const string Key_CollisionDamage = "CollisionDamage";

        public CollisionBehaviour(Entity entity)
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

                Attribute<Entity> owner = entity[ProjectileBehaviour.Key_ProjectileOwner] as Attribute<Entity>;
                Attribute<Entity> otherOwner = other[ProjectileBehaviour.Key_ProjectileOwner] as Attribute<Entity>;
                if ((owner != null && owner.Value == other) || (otherOwner != null && otherOwner.Value == entity)) {
                    continue;
                }

                Attribute<Rectangle> otherBounds = other[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;
                if (isColliding(otherBounds)) {
                    Console.WriteLine("[" + this.GetType().Name + "] " + entity.Name + " collided with " + other.Name
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
            Attribute<Rectangle> bounds = entity[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;

            return bounds.Value.Left <= other.Value.Right
                && bounds.Value.Top <= other.Value.Bottom
                && bounds.Value.Right >= other.Value.Left
                && bounds.Value.Bottom >= other.Value.Top;
        }
    }
}
