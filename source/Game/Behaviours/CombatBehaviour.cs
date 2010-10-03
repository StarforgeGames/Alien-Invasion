using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.Messages;
using Game.Processes;

namespace Game.Behaviours
{
    class CombatBehaviour : IBehaviour
    {
        public readonly int Key_IsDead;
        public readonly int Key_IsFiring;
        public readonly int Key_FiringSpeed;
        public readonly int Key_TimeSinceLastShot;

        private Entity entity;

        public CombatBehaviour(Entity entity, float firingSpeed)
        {
            this.entity = entity;

            Key_IsDead = entity.AddAttribute(new Attribute<bool>(false));
            Key_IsFiring = entity.AddAttribute(new Attribute<bool>(false));
            Key_FiringSpeed = entity.AddAttribute(new Attribute<float>(firingSpeed));
            Key_TimeSinceLastShot = entity.AddAttribute(new Attribute<float>(0f));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() {
            typeof(FireWeaponMessage)
        };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            Attribute<bool> isFiring = (Attribute<bool>) entity[Key_IsFiring];

            if (isFiring) {
                Attribute<float> firingSpeed = (Attribute<float>)entity[Key_FiringSpeed];
                Attribute<float> timeSinceLastShot = (Attribute<float>)entity[Key_TimeSinceLastShot];

                timeSinceLastShot.Value += deltaTime;

                if (timeSinceLastShot >= firingSpeed) {
                    timeSinceLastShot.Value = 0f;
                    Console.WriteLine("---------- PEW PEW! ----------");
                    // TODO: Fire shot
                }
            }
        }

        public void OnMessage(Messages.Message msg)
        {
            switch (msg.Type) {
                case FireWeaponMessage.START_FIRING: {
                    Attribute<bool> isFiring = (Attribute<bool>)entity[Key_IsFiring];
                    isFiring.Value = true;
                    break;
                }
                case FireWeaponMessage.STOP_FIRING: {
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

        #endregion
    }
}
