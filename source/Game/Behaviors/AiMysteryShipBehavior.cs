using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;
using System;

namespace Game.Behaviors
{
    public class AiMysteryShipBehavior : AEntityBasedBehavior
    {
        private float phasePosition;
        private float oldXPosition;
        
        public AiMysteryShipBehavior(Entity entity)
            : base(entity)
        {
            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        { }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position];
            if (oldXPosition == 0) {
                oldXPosition = position.Value.X;
            }

            float diff = Math.Abs(oldXPosition - position.Value.X);
            phasePosition = (phasePosition + diff) % 360;
            oldXPosition = position.Value.X;

            position.Value.Y += (float)(75 * Math.Sin(phasePosition * Math.PI / 180)) * deltaTime;

            if (position.Value.X < -100 || position.Value.X > game.World.Width + 100) {
                var destroyEntity = new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID);
                eventManager.QueueEvent(destroyEntity);
            }
        }

        public override void OnEvent(Event evt)
        { }
    }
}
