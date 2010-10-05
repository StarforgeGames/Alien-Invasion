using System;
using Game.Entities;
using Game.Messages;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Behaviours
{

    class RenderBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_Sprite = "Sprite";

        public RenderBehaviour(Entity entity, string sprite)
            : base(entity)
        {
            entity.AddAttribute(Key_Sprite, new Attribute<string>(sprite));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() { };
        public override ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnMessage(Message msg)
        {
        }

        #endregion
    }

}
