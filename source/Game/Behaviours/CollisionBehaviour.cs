using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Entities;

namespace Game.Behaviours
{
    class CollisionBehaviour : IBehaviour
    {
        private Entity entity;

        public CollisionBehaviour(Entity entity)
        {
            this.entity = entity;
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() { };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public void OnUpdate(float deltaTime)
        {

        }

        public void OnMessage(Messages.Message msg)
        {

        }

        #endregion
    }
}
