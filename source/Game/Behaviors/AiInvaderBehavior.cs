using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    public class AiInvaderBehavior : AEntityBasedBehavior
    {
        public AiInvaderBehavior(Entity entity)
            : base(entity)
        { 
            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        { }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<Vector2D> atBoundary = entity[SpatialBehavior.Key_AtBoundary];

            if (atBoundary.Value.X < 0 || atBoundary.Value.X > 0 || atBoundary.Value.Y < 0 || atBoundary.Value.Y > 0) {
                eventManager.QueueEvent(AiUpdateMovementEvent.AtBorder(entity.ID, new Vector2D(atBoundary.Value)));
            }
        }

        public override void OnEvent(Event evt)
        { }
    }
}
