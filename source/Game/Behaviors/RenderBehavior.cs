using System;
using System.Collections.Generic;
using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{

    class RenderBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Sprite = "Sprite";
        public const string Key_Material = "Material";
        public const string Key_IsRenderable = "IsRenderable";

        public RenderBehavior(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>();

            entity.AddAttribute(Key_Sprite, new Attribute<string>(String.Empty));
            entity.AddAttribute(Key_Material, new Attribute<string>(String.Empty));
            entity.AddAttribute(Key_IsRenderable, new Attribute<bool>(true));
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnEvent(Event evt)
        {
        }
    }

}
