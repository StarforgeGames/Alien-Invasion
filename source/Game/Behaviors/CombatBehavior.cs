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
            entity.AddAttribute(Key_IsFiring, new Attribute<bool>(false));
            entity.AddAttribute(Key_IsSingleShot, new Attribute<bool>(false));
            entity.AddAttribute(Key_FiringSpeed, new Attribute<float>(0));
            entity.AddAttribute(Key_TimeSinceLastShot, new Attribute<float>(999));
            entity.AddAttribute(Key_ProjectileType, new Attribute<string>(string.Empty));

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

            Attribute<bool> isFiring = entity[Key_IsFiring];
            Attribute<bool> isSingleShot = entity[Key_IsSingleShot];

            if (isFiring || isSingleShot) 
            {
                Attribute<float> firingSpeed = entity[Key_FiringSpeed];
                Attribute<float> timeSinceLastShot = entity[Key_TimeSinceLastShot];

                timeSinceLastShot.Value += deltaTime;

                if (timeSinceLastShot >= firingSpeed || isSingleShot) 
                {
                    timeSinceLastShot.Value = 0.0f;
                    createProjectileAtCurrentPosition();
                    if (entity.Type == "player")
                    {
                        startAnimation();
                    }

                    if (isSingleShot) 
                    {
                        isSingleShot.Value = false;
                    }

                    Console.WriteLine("[" + this.GetType().Name + "] Firing weapon of " + entity);
                }
            }
        }

        private void startAnimation()
        {
            eventManager.QueueEvent(AnimationEvent.Play(entity.ID));
        }

        private void createProjectileAtCurrentPosition()
        {
            var projectileType = entity[Key_ProjectileType];
            var evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, projectileType);

            var owner = new Attribute<Entity>(entity);
            evt.AddAttribute(ProjectileBehavior.Key_ProjectileOwner, owner);
            var faction = entity[CollisionBehavior.Key_Faction];
            evt.AddAttribute(CollisionBehavior.Key_Faction, faction);

            var position = entity[SpatialBehavior.Key_Position];
            var dimensions = entity[SpatialBehavior.Key_Dimensions];

            float startX = position.Value.X + (dimensions.Value.X / 2f) - 2.5f;
            float startY = position.Value.Y + (dimensions.Value.Y / 2f);
            var pewpewPosition = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, pewpewPosition);

            var pewpewDimensions = new Attribute<Vector2D>(new Vector2D(5, 15));
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, pewpewDimensions);

            eventManager.QueueEvent(evt);
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) 
            {
                case FireWeaponEvent.FIRE_SINGLE_SHOT: 
                {
                    Attribute<bool> isSingleShot = entity[Key_IsSingleShot];
                    isSingleShot.Value = true;

                    break;
                }
                case FireWeaponEvent.START_FIRING: 
                {
                    Attribute<bool> isFiring = entity[Key_IsFiring];
                    isFiring.Value = true;
                    break;
                }
                case FireWeaponEvent.STOP_FIRING: 
                {
                    Attribute<bool> isFiring = entity[Key_IsFiring];
                    isFiring.Value = false;
                        
                    // Allow that a shot can be fired faster when the fire button is hit again in rapid succession
                    Attribute<float> timeSinceLastShot = entity[Key_TimeSinceLastShot];
                    Attribute<float> firingSpeed = entity[Key_FiringSpeed];
                    timeSinceLastShot.Value += firingSpeed / 2;
                    break;
                }
            }
        }
    }
}
