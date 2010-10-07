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
    class CollisionBehaviour : IBehaviour
    {
        // Attribute Keys
        public const string Key_IsPhysical = "IsPhysical";
        public const string Key_CollisionDamage = "CollisionDamage";

        private Entity entity;

        public CollisionBehaviour(Entity entity, int collisionDamage)
        {
            this.entity = entity;
            entity.AddAttribute(Key_IsPhysical, new Attribute<bool>(true));
            entity.AddAttribute(Key_CollisionDamage, new Attribute<int>(collisionDamage));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() { };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get { return supportedMessages.AsReadOnly(); }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity e in entity.Game.Entities) {
                if (entity == e) {
                    continue;
                }

                Attribute<bool> isPhysical = e[Key_IsPhysical] as Attribute<bool>;
                if (isPhysical == null || !isPhysical) {
                    continue;
                }

                Attribute<Entity> owner = entity[ProjectileBehaviour.Key_ProjectileOwner] as Attribute<Entity>;
                Attribute<Entity> otherOwner = e[ProjectileBehaviour.Key_ProjectileOwner] as Attribute<Entity>;
                if ((owner != null && owner.Value == e) || (otherOwner != null && otherOwner.Value == entity)) {
                    continue;
                }
                
                Attribute<Rectangle> otherBounds = e[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;
                if (isColliding(otherBounds)) {
                    Console.WriteLine(entity.Name + " collides with " + e.Name + "!");

                    CollisionEvent collisionMsg = new CollisionEvent(CollisionEvent.ACTOR_COLLIDES, e);
                    entity.EventManager.QueueEvent(collisionMsg);

                    Attribute<int> collisionDmg = e[Key_CollisionDamage] as Attribute<int>;
                    DamageEvent dmgMsg = new DamageEvent(DamageEvent.RECEIVE_DAMAGE, collisionDmg, e);
                    entity.EventManager.QueueEvent(dmgMsg);
                }
            }
        }

        public void OnMessage(Event msg)
        {

        }

        #endregion

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
