using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    public class CombatBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_IsFiring = "IsFiring";
        public const string Key_IsSingleShot = "IsSingleShot";
        public const string Key_FiringSpeed = "FiringSpeed";
        public const string Key_TimeSinceLastShot = "TimeSinceLastShot";
        public const string Key_ProjectileType = "ProjectileType";

        public CombatBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_IsFiring, false);
            entity.AddAttribute(Key_IsSingleShot, false);
            entity.AddAttribute(Key_FiringSpeed, 0);
            entity.AddAttribute(Key_TimeSinceLastShot, 999);
            entity.AddAttribute(Key_ProjectileType, string.Empty);

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(FireWeaponEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (entity.IsDead) 
            {
                return;
            }

            bool isFiring = entity[Key_IsFiring];
            bool isSingleShot = entity[Key_IsSingleShot];

            if (isFiring || isSingleShot) 
            {
                float firingSpeed = entity[Key_FiringSpeed];
                float timeSinceLastShot = entity[Key_TimeSinceLastShot];

                timeSinceLastShot += deltaTime;

                if (timeSinceLastShot >= firingSpeed || isSingleShot) 
                {
                    timeSinceLastShot = 0.0f;
                    createProjectileAtCurrentPosition();
                    if (entity.Type == "player")
                    {
                        startAnimation();
                    }

                    if (isSingleShot) 
                    {
                        isSingleShot = false;
                        entity[Key_IsSingleShot] = isSingleShot;
                    }

                    Console.WriteLine("[" + this.GetType().Name + "] Firing weapon of " + entity);
                }

                entity[Key_TimeSinceLastShot] = timeSinceLastShot;
            }
        }

        private void startAnimation()
        {
            eventManager.QueueEvent(AnimationEvent.Play(entity.ID));
        }

        private void createProjectileAtCurrentPosition()
        {
            string projectileType = entity[Key_ProjectileType];
            var evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, projectileType);

            Entity owner = entity;
            evt.AddAttribute(ProjectileBehavior.Key_ProjectileOwner, owner);
            string faction = entity[CollisionBehavior.Key_Faction];
            evt.AddAttribute(CollisionBehavior.Key_Faction, faction);

            Vector2D position = entity[SpatialBehavior.Key_Position];
            Vector2D dimensions = entity[SpatialBehavior.Key_Dimensions];

            float startX = position.X + (dimensions.X / 2f) - 2.5f;
            float startY = position.Y + (dimensions.Y / 2f);
            Vector2D pewpewPosition = new Vector2D(startX, startY);
            evt.AddAttribute(SpatialBehavior.Key_Position, pewpewPosition);

            Vector2D pewpewDimensions = new Vector2D(5, 15);
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, pewpewDimensions);

            eventManager.QueueEvent(evt);
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) 
            {
                case FireWeaponEvent.FIRE_SINGLE_SHOT: 
                {
                    entity[Key_IsSingleShot] = true;
                    break;
                }
                case FireWeaponEvent.START_FIRING: 
                {
                    entity[Key_IsFiring] = true;
                    break;
                }
                case FireWeaponEvent.STOP_FIRING: 
                {
                    entity[Key_IsFiring] = false;
                        
                    // Allow that a shot can be fired faster when the fire button is hit again in rapid succession
                    float timeSinceLastShot = entity[Key_TimeSinceLastShot];
                    timeSinceLastShot += 0.3f;
                    entity[Key_TimeSinceLastShot] = timeSinceLastShot;
                    break;
                }
            }
        }
    }
}
