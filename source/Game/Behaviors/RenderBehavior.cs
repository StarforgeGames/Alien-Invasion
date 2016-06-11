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
        public const string Key_Frame = "Frame";

        public RenderBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Material, (ResourceHandle)null);
            entity.AddAttribute(Key_Mesh, (ResourceHandle)null);
            entity.AddAttribute(Key_IsRenderable, true);
            entity.AddAttribute(Key_Frame, 0.0f);

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        { }
    }

}
