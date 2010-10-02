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

        private Entity entity;
        private float firingSpeed;
        private bool isFiring = false;

        private Process process = null;

        public CombatBehaviour(Entity entity, float firingSpeed)
        {
            this.entity = entity;
            this.firingSpeed = firingSpeed;

            Key_IsDead = entity.NextAttributeID;
            entity.AddAttribute(Key_IsDead, false);
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
            if (isFiring && process == null) {
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
                case FireWeaponMessage.START_FIRING:
                    isFiring = true;
                    break;
                case FireWeaponMessage.STOP_FIRING:
                    isFiring = false;
                    break;
            }
        }

        #endregion
    }
}
