using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Processes;
using Game.Utility;

namespace Game.Behaviours
{
    class CombatBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_IsFiring = "IsFiring";
        public const string Key_FiringSpeed = "FiringSpeed";
        public const string Key_TimeSinceLastShot = "TimeSinceLastShot";

        public CombatBehaviour(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { typeof(FireWeaponEvent) };

            entity.AddAttribute(Key_IsFiring, new Attribute<bool>(false));
            entity.AddAttribute(Key_FiringSpeed, new Attribute<float>(0));
            entity.AddAttribute(Key_TimeSinceLastShot, new Attribute<float>(0));
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<bool> isFiring = (Attribute<bool>) entity[Key_IsFiring];

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
            evt.AddAttribute(ProjectileBehaviour.Key_ProjectileOwner, owner);

            Attribute<Vector2D> position = entity[SpatialBehaviour.Key_Position] as Attribute<Vector2D>;
            Attribute<Rectangle> bounds = entity[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;

            float startX = position.Value.X + (bounds.Value.Width / 2f);
            float startY = position.Value.Y - (bounds.Value.Height / 2f);
            Attribute<Vector2D> pewpewPosition = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehaviour.Key_Position, pewpewPosition);

            Rectangle rect = new Rectangle(pewpewPosition, 5, 15);
            Attribute<Rectangle> pewpewBounds = new Attribute<Rectangle>(rect);
            evt.AddAttribute(SpatialBehaviour.Key_Bounds, pewpewBounds);

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
