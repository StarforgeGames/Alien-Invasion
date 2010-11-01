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
    class SpatialBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Position = "Position";
        public const string Key_Dimensions = "Dimensions";
        public const string Key_Orientation = "Orientation";
        public const string Key_Speed = "Speed";
        public const string Key_IsMoving = "IsMoving";

        public SpatialBehavior(Entity entity)
            : base (entity)
        {
            entity.AddAttribute(Key_Position, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_Dimensions, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_Orientation, new Attribute<Vector2D>(new Vector2D(0, 0)));
            entity.AddAttribute(Key_Speed, new Attribute<float>(0));
            entity.AddAttribute(Key_IsMoving, new Attribute<bool>(false));
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

            bool isMoving = entity[Key_IsMoving] as Attribute<bool>;

            if (isMoving) {
                Attribute<float> speed = entity[Key_Speed] as Attribute<float>;
                Attribute<Vector2D> position = entity[Key_Position] as Attribute<Vector2D>;
                Attribute<Vector2D> direction = entity[Key_Orientation] as Attribute<Vector2D>;

                position.Value.X += direction.Value.X * speed * deltaTime;
                position.Value.Y += direction.Value.Y * speed * deltaTime;
                checkBounds(position);
            }
        }

        private void checkBounds(Attribute<Vector2D> position)
        {
            Attribute<Vector2D> dimensions = entity[Key_Dimensions] as Attribute<Vector2D>;

            if (position.Value.X <= 0) {
                position.Value.X = 0;
                setMovement(false, Direction.West);
            }
            else if ((position.Value.X + dimensions.Value.X) > entity.Game.WorldWidth) {
                position.Value.X = entity.Game.WorldWidth - dimensions.Value.X;
                setMovement(false, Direction.East);
            }

            if (position.Value.Y <= 0) {
                position.Value.Y = 0;
                setMovement(false, Direction.North);
            }
            else if ((position.Value.Y + dimensions.Value.Y) >= entity.Game.WorldHeight) {
                position.Value.Y = entity.Game.WorldHeight - dimensions.Value.Y;
                setMovement(false, Direction.South);
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case MoveEvent.START_MOVING: {
                    MoveEvent moveMsg = (MoveEvent)evt;
                    setMovement(true, moveMsg.Direction);
                    break;
                }
                case MoveEvent.STOP_MOVING: {
                    MoveEvent moveMsg = (MoveEvent)evt;
                    setMovement(false, moveMsg.Direction);
                    break;
                }
            }
        }

        private void setMovement(bool isMoving, Direction direction)
        {
            Attribute<Vector2D> entityDirection = entity[Key_Orientation] as Attribute<Vector2D>;
            switch (direction) {
                case Direction.North:
                    entityDirection.Value.Y = isMoving ? -1f : 0f;
                    break;
                case Direction.West:
                    entityDirection.Value.X = isMoving ? -1f : 0f;
                    break;
                case Direction.South:
                    entityDirection.Value.Y = isMoving ? 1f : 0f;
                    break;
                case Direction.East:
                    entityDirection.Value.X = isMoving ? 1f : 0f;
                    break;
            }

            Attribute<bool> entityIsMoving = entity[Key_IsMoving] as Attribute<bool>;
            if (entityDirection.Value.X != 0f || entityDirection.Value.Y != 0f) {
                entityIsMoving.Value = true;
            }
            else {
                entityIsMoving.Value = false;
            }
        }
    }

}
