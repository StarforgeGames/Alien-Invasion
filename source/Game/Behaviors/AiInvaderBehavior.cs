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
            Vector2D atBoundary = entity[SpatialBehavior.Key_AtBoundary];

            if (atBoundary.X < 0 || atBoundary.X > 0 || atBoundary.Y < 0 || atBoundary.Y > 0) {
                eventManager.QueueEvent(AiUpdateMovementEvent.AtBorder(entity.ID, atBoundary));
            }
        }

        public override void OnEvent(Event evt)
        { }
    }
}
