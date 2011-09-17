using System;
using Game.Entities;
using Game.EventManagement.Events;
using ResourceManagement;

namespace Game.Behaviors
{

    public class RenderBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Material = "Material";
        public const string Key_Mesh = "Mesh";
        public const string Key_IsRenderable = "IsRenderable";

        public RenderBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Material, new Attribute<ResourceHandle>(null));
            entity.AddAttribute(Key_Mesh, new Attribute<ResourceHandle>(null));
            entity.AddAttribute(Key_IsRenderable, new Attribute<bool>(true));

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        { }
    }

}
