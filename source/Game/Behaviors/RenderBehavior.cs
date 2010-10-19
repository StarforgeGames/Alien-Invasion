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
        public const string Key_Renderable = "renderable";

        public RenderBehavior(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>();

            entity.AddAttribute(Key_Sprite, new Attribute<string>(String.Empty));
            entity.AddAttribute(Key_Renderable, new Attribute<bool>(true));
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEvent(Event evt)
        {
        }
    }

}
