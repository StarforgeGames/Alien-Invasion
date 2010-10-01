using System;
using Game.Entities;
using Game.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Behaviours
{

    class RenderBehaviour : IBehaviour
    {
        List<Type> supportedMessages = new List<Type>() { };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        private Entity entity;

        public readonly int Key_Sprite;

        public RenderBehaviour(Entity entity, string sprite)
        {
            this.entity = entity;

            Key_Sprite = entity.NextAttributeID;
            entity.AddAttribute(Key_Sprite, sprite);
        }

        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {
           
        }

        public void OnMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
