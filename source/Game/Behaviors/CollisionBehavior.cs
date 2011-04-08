using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    public class CollisionBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_IsPhysical = "IsPhysical";
        public const string Key_CollisionDamage = "CollisionDamage";

        public CollisionBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_IsPhysical, new Attribute<bool>(true));
            entity.AddAttribute(Key_CollisionDamage, new Attribute<int>(1));

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (entity.IsDead) {
                return;
            }

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

                if (isColliding(other)) {
                    Console.WriteLine("[" + this.GetType().Name + "] " + entity.Type + " collided with " + other.Type
                        + "!");

                    CollisionEvent collisionMsg = new CollisionEvent(
                        CollisionEvent.ACTOR_COLLIDES, 
                        other.ID, 
                        entity.ID);
                    EventManager.QueueEvent(collisionMsg);

                    Attribute<int> collisionDmg = entity[Key_CollisionDamage] as Attribute<int>;
                    DamageEvent dmgMsg = new DamageEvent(DamageEvent.RECEIVE_DAMAGE,
                        entity.ID,
                        collisionDmg, 
                        other.ID);
                    EventManager.QueueEvent(dmgMsg);
                }
            }
        }

        public override void OnEvent(Event evt)
        {
        }

        private bool isColliding(Entity other)
        {
            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            Attribute<Vector2D> dimensions = entity[SpatialBehavior.Key_Dimensions] as Attribute<Vector2D>;

            Attribute<Vector2D> otherPosition = other[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            Attribute<Vector2D> otherDimensions = other[SpatialBehavior.Key_Dimensions] as Attribute<Vector2D>;

            return position.Value.X <= otherPosition.Value.X + otherDimensions.Value.X
                && position.Value.Y <= otherPosition.Value.Y + otherDimensions.Value.Y
                && position.Value.X + dimensions.Value.X >= otherPosition.Value.X
                && position.Value.Y + dimensions.Value.Y >= otherPosition.Value.Y;
        }
    }
}
