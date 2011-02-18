using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    class CombatBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_IsFiring = "IsFiring";
        public const string Key_FiringSpeed = "FiringSpeed";
        public const string Key_TimeSinceLastShot = "TimeSinceLastShot";

        public CombatBehavior(Entity entity)
            : base(entity)
        { 
            entity.AddAttribute(Key_IsFiring, new Attribute<bool>(false));
            entity.AddAttribute(Key_FiringSpeed, new Attribute<float>(0));
            entity.AddAttribute(Key_TimeSinceLastShot, new Attribute<float>(999));

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(FireWeaponEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (entity.IsDead) {
                return;
            }

            Attribute<bool> isFiring = (Attribute<bool>)entity[Key_IsFiring];

            if (isFiring) {
                Attribute<float> firingSpeed = (Attribute<float>)entity[Key_FiringSpeed];
                Attribute<float> timeSinceLastShot = (Attribute<float>)entity[Key_TimeSinceLastShot];

                timeSinceLastShot.Value += deltaTime;

                if (timeSinceLastShot >= firingSpeed) {
                    timeSinceLastShot.Value = 0f;
                    createProjectileAtCurrentPosition();

                    Console.WriteLine("[" + this.GetType().Name + "] Firing weapon of " + entity);
                }
            }
        }

        private void createProjectileAtCurrentPosition()
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "pewpew");

            Attribute<Entity> owner = new Attribute<Entity>(entity);
            evt.AddAttribute(ProjectileBehavior.Key_ProjectileOwner, owner);

            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            Attribute<Vector2D> dimensions = entity[SpatialBehavior.Key_Dimensions] as Attribute<Vector2D>;

            float startX = position.Value.X + (dimensions.Value.X / 2f) - 2.5f;
            float startY = position.Value.Y + (dimensions.Value.Y / 2f);
            Attribute<Vector2D> pewpewPosition = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, pewpewPosition);

            Attribute<Vector2D> pewpewDimensions = new Attribute<Vector2D>(new Vector2D(5, 15));
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, pewpewDimensions);

            EventManager.QueueEvent(evt);
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case FireWeaponEvent.START_FIRING: {
                    Attribute<bool> isFiring = entity[Key_IsFiring] as Attribute<bool>;
                    isFiring.Value = true;
                    break;
                }
                case FireWeaponEvent.STOP_FIRING: {
                    Attribute<bool> isFiring = entity[Key_IsFiring] as Attribute<bool>;
                    isFiring.Value = false;

                    // Set to firing speed so that a shot is immediately fired when the fire button is hit again
                    Attribute<float> timeSinceLastShot = entity[Key_TimeSinceLastShot] as Attribute<float>;
                    Attribute<float> firingSpeed = entity[Key_FiringSpeed] as Attribute<float>;
                    timeSinceLastShot.Value = firingSpeed;
                    break;
                }
            }
        }
    }
}
