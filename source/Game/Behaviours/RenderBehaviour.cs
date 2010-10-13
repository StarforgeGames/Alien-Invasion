using System;
using Game.Entities;
using Game.EventManagement.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Behaviours
{

    class RenderBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_Sprite = "Sprite";

        public RenderBehaviour(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>();

            entity.AddAttribute(Key_Sprite, new Attribute<string>(String.Empty));
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEvent(Event evt)
        {
        }
    }

}
