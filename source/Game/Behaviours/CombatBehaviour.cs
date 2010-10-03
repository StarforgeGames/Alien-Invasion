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

        private Entity entity;

        private Process process = null;

        public CombatBehaviour(Entity entity, float firingSpeed)
        {
            this.entity = entity;

            Key_IsDead = entity.AddAttribute(new Attribute<bool>(false));
            Key_IsFiring = entity.AddAttribute(new Attribute<bool>(false));
            Key_FiringSpeed = entity.AddAttribute(new Attribute<float>(firingSpeed));
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

            if (isFiring && process == null) {
                Attribute<float> firingSpeed = (Attribute<float>)entity[Key_FiringSpeed];
                process = new FireWeaponProcess(entity, firingSpeed);
                entity.Game.ProcessManager.Attach(process);
            }
            else if (!isFiring && process != null) {
                entity.Game.ProcessManager.Detach(process);
                process = null;
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
                    break;
                }
            }
        }

        #endregion
    }
}
