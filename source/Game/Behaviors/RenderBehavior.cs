using System;
using Game.Entities;
using Game.EventManagement.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game.Behaviors
{

    class RenderBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Sprite = "Sprite";

        public RenderBehavior(Entity entity)
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
