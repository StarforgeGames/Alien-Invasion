using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    /// <summary>
    /// Behaviour that gives an entity the feature if having a position in the world and allowing it to move around
    /// in the world.
    /// </summary>
    public class SpatialBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Position = "Position";
        public const string Key_Dimensions = "Dimensions";
        public const string Key_MoveDirection = "MoveDirection";
        public const string Key_Speed = "Speed";
        public const string Key_AtBoundary = "AtBoundary";
        public const string Key_RespectsBoundary = "RespectsBoundary";

        public SpatialBehavior(Entity entity)
            : base (entity)
        {
            entity.AddAttribute(Key_Position, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_Dimensions, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_MoveDirection, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_Speed, new Attribute<float>(0));
            entity.AddAttribute(Key_AtBoundary, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_RespectsBoundary, new Attribute<bool>(true));

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(MoveEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (entity.IsDead) {
                return;
            }

            Attribute<float> speed = entity[Key_Speed];
            Attribute<Vector2D> position = entity[Key_Position];
            Attribute<Vector2D> direction = entity[Key_MoveDirection];

            position.Value.X += direction.Value.X * speed * deltaTime;
            position.Value.Y += direction.Value.Y * speed * deltaTime;

            Attribute<bool> respectBoundary = entity[Key_RespectsBoundary];
            if (respectBoundary) {
                checkAndEnforceBounds(position);
            }
        }

        private void checkAndEnforceBounds(Attribute<Vector2D> position)
        {
            Attribute<Vector2D> dimensions = entity[Key_Dimensions];
            Attribute<Vector2D> direction = entity[Key_MoveDirection];
            Attribute<Vector2D> atBoundary = entity[Key_AtBoundary];

            if (position.Value.X <= 0.0f) {
                position.Value.X = 0.0f;
                direction.Value.X = 0.0f;
                atBoundary.Value.X = -1.0f;
            }
            else if ((position.Value.X + dimensions.Value.X) > world.Width) {
                position.Value.X = world.Width - dimensions.Value.X;
                direction.Value.X = 0.0f;
                atBoundary.Value.X = 1.0f;
            }
            else {
                atBoundary.Value.X = 0.0f;
            }

            if (position.Value.Y <= 0.0f) {
                position.Value.Y = 0.0f;
                direction.Value.Y = 0.0f;
                atBoundary.Value.Y = -1.0f;
            }
            else if ((position.Value.Y + dimensions.Value.Y) >= world.Height) {
                position.Value.Y = world.Height - dimensions.Value.Y;
                direction.Value.Y = 0.0f;
                atBoundary.Value.Y = 1.0f;
            }
            else {
                atBoundary.Value.Y = 0.0f;
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case MoveEvent.START_MOVING: {
                    MoveEvent moveMsg = (MoveEvent)evt;
                    setDirection(moveMsg.Direction);
                    break;
                }
                case MoveEvent.STOP_MOVING: {
                    MoveEvent moveMsg = (MoveEvent)evt;
                    setDirection(moveMsg.Direction);
                    break;
                }
            }
        }

        private void setDirection(Vector2D newDirection)
        {
            Attribute<Vector2D> entityDirection = entity[Key_MoveDirection];
            entityDirection.Value.X = newDirection.X;
            entityDirection.Value.Y = newDirection.Y;
        }
    }

}
