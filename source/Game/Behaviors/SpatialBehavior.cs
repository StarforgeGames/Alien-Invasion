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
        public const string Key_MovementSpeed = "MovementSpeed";
        public const string Key_AtBoundary = "AtBoundary";
        public const string Key_RespectsBoundary = "RespectsBoundary";

        public SpatialBehavior(Entity entity)
            : base (entity)
        {
            entity.AddAttribute(Key_Position, new Vector2D(0, 0));
            entity.AddAttribute(Key_Dimensions, new Vector2D(0, 0));
            entity.AddAttribute(Key_MoveDirection, new Vector2D(0, 0));
            entity.AddAttribute(Key_MovementSpeed, 0);
            entity.AddAttribute(Key_AtBoundary, new Vector2D(0, 0));
            entity.AddAttribute(Key_RespectsBoundary, true);

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

            float speed = entity[Key_MovementSpeed];
            Vector2D position = entity[Key_Position];
            Vector2D direction = entity[Key_MoveDirection];

            position.X += direction.X * speed * deltaTime;
            position.Y += direction.Y * speed * deltaTime;
            entity[Key_Position] = position;

            bool respectsBoundary = entity[Key_RespectsBoundary];
            if (respectsBoundary) {
                checkAndEnforceBounds(position);
            }
        }

        private void checkAndEnforceBounds(Vector2D position)
        {
            Vector2D dimensions = entity[Key_Dimensions];
            Vector2D direction = entity[Key_MoveDirection];
            Vector2D atBoundary = entity[Key_AtBoundary];

            if (position.X <= 0.0f) {
                position.X = 0.0f;
                direction.X = 0.0f;
                atBoundary.X = -1.0f;
            }
            else if ((position.X + dimensions.X) > world.Width) {
                position.X = world.Width - dimensions.X;
                direction.X = 0.0f;
                atBoundary.X = 1.0f;
            }
            else {
                atBoundary.X = 0.0f;
            }

            if (position.Y <= 0.0f) {
                position.Y = 0.0f;
                direction.Y = 0.0f;
                atBoundary.Y = -1.0f;
            }
            else if ((position.Y + dimensions.Y) >= world.Height) {
                position.Y = world.Height - dimensions.Y;
                direction.Y = 0.0f;
                atBoundary.Y = 1.0f;
            }
            else {
                atBoundary.Y = 0.0f;
            }

            entity[Key_Position] = position;
            entity[Key_MoveDirection] = direction;
            entity[Key_AtBoundary] = atBoundary;
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
            Vector2D entityDirection = entity[Key_MoveDirection];
            entityDirection.X = newDirection.X;
            entityDirection.Y = newDirection.Y;
            entity[Key_MoveDirection] = entityDirection;
        }
    }

}
