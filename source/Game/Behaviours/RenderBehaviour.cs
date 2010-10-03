using System;
using Game.Entities;
using Game.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Behaviours
{

    class RenderBehaviour : IBehaviour
    {
        public readonly int Key_Sprite;

        private Entity entity;

        public RenderBehaviour(Entity entity, string sprite)
        {
            this.entity = entity;

            Key_Sprite = entity.AddAttribute(new Attribute<string>(sprite));
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

        public void OnMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
