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

        public CombatBehaviour(Entity entity, float firingSpeed)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { typeof(FireWeaponEvent) };

            entity.AddAttribute(Key_IsFiring, new Attribute<bool>(false));
            entity.AddAttribute(Key_FiringSpeed, new Attribute<float>(firingSpeed));
            entity.AddAttribute(Key_TimeSinceLastShot, new Attribute<float>(firingSpeed));
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

                    Entity pewpew = entity.Game.AddEntity("pewpew");
                    Attribute<Entity> owner = pewpew[ProjectileBehaviour.Key_ProjectileOwner] as Attribute<Entity>;
                    owner.Value = entity;

                    Attribute<Vector2D> position = entity[SpatialBehaviour.Key_Position] as Attribute<Vector2D>;
                    Attribute<Rectangle> bounds = entity[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;

                    Attribute<Vector2D> pewpewPosition = pewpew[SpatialBehaviour.Key_Position] as Attribute<Vector2D>;
                    Attribute<Rectangle> pewpewBounds = pewpew[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;
                    pewpewPosition.Value.X = position.Value.X + (bounds.Value.Width / 2f);
                    pewpewPosition.Value.Y = position.Value.Y - (pewpewBounds.Value.Height / 2f);

                    pewpew.EventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, Direction.North));

                    Console.WriteLine("PEW PEW at (" + pewpewPosition.Value.X + "/" + pewpewPosition.Value.Y + ")!");
                }
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case FireWeaponEvent.START_FIRING: {
                    Attribute<bool> isFiring = (Attribute<bool>)entity[Key_IsFiring];
                    isFiring.Value = true;
                    break;
                }
                case FireWeaponEvent.STOP_FIRING: {
                    Attribute<bool> isFiring = (Attribute<bool>)entity[Key_IsFiring];
                    isFiring.Value = false;

                    // Set to firing speed so that a shot is immediately fired when the fire button is hit again
                    Attribute<float> timeSinceLastShot = (Attribute<float>)entity[Key_TimeSinceLastShot];
                    Attribute<float> firingSpeed = (Attribute<float>)entity[Key_FiringSpeed];
                    timeSinceLastShot.Value = firingSpeed;
                    break;
                }
            }
        }
    }
}
