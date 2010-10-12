using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviours
{
    /// <summary>
    /// Behaviour that gives an entity the feature if having a position in the world and allowing it to move around
    /// in the world.
    /// </summary>
    class SpatialBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_Position = "Position";
        public const string Key_Bounds = "Bounds";
        public const string Key_Orientation = "Orientation";
        public const string Key_Speed = "Speed";
        public const string Key_IsMoving = "IsMoving";

        public SpatialBehaviour(Entity entity)
            : base (entity)
        {
            handledEventTypes = new List<Type>() { typeof(MoveEvent) };

            Vector2D position = Vector2D.Empty;
            entity.AddAttribute(Key_Position, new Attribute<Vector2D>(position));
            Rectangle bounds = new Rectangle(position, 0, 0);
            entity.AddAttribute(Key_Bounds, new Attribute<Rectangle>(bounds));
            entity.AddAttribute(Key_Orientation, new Attribute<Vector2D>(Vector2D.Empty));
            entity.AddAttribute(Key_Speed, new Attribute<float>(0));
            entity.AddAttribute(Key_IsMoving, new Attribute<bool>(false));
        }

        public override void OnUpdate(float deltaTime)
        {
            bool isMoving = entity[Key_IsMoving] as Attribute<bool>;

            if (isMoving) {
                Attribute<float> speed = entity[Key_Speed] as Attribute<float>;
                Attribute<Vector2D> position = entity[Key_Position] as Attribute<Vector2D>;
                Attribute<Vector2D> direction = entity[Key_Orientation] as Attribute<Vector2D>;

                position.Value.X += direction.Value.X * speed * deltaTime;
                position.Value.Y += direction.Value.Y * speed * deltaTime;
                checkBounds(position);

                Console.WriteLine("[" + this.GetType().Name +"] " + entity.Name + " moved " + direction.ToString() 
                    + " to (" + position.Value.X + "/" + position.Value.Y + ")");
            }
        }

        private void checkBounds(Attribute<Vector2D> position)
        {
            Attribute<Rectangle> bounds = entity[Key_Bounds] as Attribute<Rectangle>;

            if (bounds.Value.Left <= 0) {
                position.Value.X = 0;
                setMovement(false, Direction.West);
            }
            else if (bounds.Value.Right > entity.Game.WorldWidth) {
                position.Value.X = entity.Game.WorldWidth - bounds.Value.Width;
                setMovement(false, Direction.East);
            }

            if (bounds.Value.Top <= 0) {
                position.Value.Y = 0;
                setMovement(false, Direction.North);
            }
            else if (bounds.Value.Bottom >= entity.Game.WorldHeight) {
                position.Value.Y = entity.Game.WorldHeight - bounds.Value.Height;
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
