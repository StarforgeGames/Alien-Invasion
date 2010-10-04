using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.Messages;
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
        public const string Key_Orientation = "Orientation";
        public const string Key_Speed = "Speed";
        public const string Key_IsMoving = "IsMoving";
        
        public SpatialBehaviour(Entity entity, float positionX, float positionY, float speed)
            : base (entity)
        {
            Vector2D pos = new Vector2D(positionX, positionY);
            entity.AddAttribute(Key_Position, new Attribute<Vector2D>(pos));
            entity.AddAttribute(Key_Orientation, new Attribute<Vector2D>(Vector2D.Empty));
            entity.AddAttribute(Key_Speed, new Attribute<float>(speed));
            entity.AddAttribute(Key_IsMoving, new Attribute<bool>(false));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() {
            typeof(MoveMessage)
        };
        public override ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            bool isMoving = entity[Key_IsMoving] as Attribute<bool>;

            if (isMoving) {
                Attribute<float> speed = entity[Key_Speed] as Attribute<float>;
                Attribute<Vector2D> position = entity[Key_Position] as Attribute<Vector2D>;
                Attribute<Vector2D> direction = entity[Key_Orientation] as Attribute<Vector2D>;

                position.Value = position + (direction.Value * speed * deltaTime);
                checkBounds(position);

                Console.WriteLine(entity.Name + " moved " + direction.ToString() + " to (" 
                    + position.Value.X + "/" + position.Value.Y + ")");
            }
        }

        public override void OnMessage(Message msg)
        {
            switch (msg.Type) {
                case MoveMessage.START_MOVING: {
                    MoveMessage moveMsg = (MoveMessage)msg;
                    setMovement(true, moveMsg.Direction);
                    break;
                }
                case MoveMessage.STOP_MOVING: {
                    MoveMessage moveMsg = (MoveMessage)msg;
                    setMovement(false, moveMsg.Direction);
                    break;
                }
            }
        }

        #endregion

        private void checkBounds(Attribute<Vector2D> position)
        {
            if (position.Value.X <= 0) {
                position.Value.X = 0;
                setMovement(false, Direction.West);
            }
            else if (position.Value.X > entity.Game.WorldWidth) {
                position.Value.X = entity.Game.WorldWidth;
                setMovement(false, Direction.East);
            }

            if (position.Value.Y <= 0) {
                position.Value.Y = 0;
                setMovement(false, Direction.North);
            }
            else if (position.Value.Y > entity.Game.WorldHeight) {
                position.Value.Y = entity.Game.WorldHeight;
                setMovement(false, Direction.South);
            }
        }

        private void setMovement(bool isMoving, Direction direction)
        {
            Attribute<bool> entityIsMoving = entity[Key_IsMoving] as Attribute<bool>;
            entityIsMoving.Value = isMoving;

            Attribute<Vector2D> entityDirection = entity[Key_Orientation] as Attribute<Vector2D>;
            switch (direction) {
                case Direction.North:
                    entityDirection.Value.Y = isMoving ? -1d : 0d;
                    break;
                case Direction.West:
                    entityDirection.Value.X = isMoving ? -1d : 0d;
                    break;
                case Direction.South:
                    entityDirection.Value.Y = isMoving ? 1d : 0d;
                    break;
                case Direction.East:
                    entityDirection.Value.X = isMoving ? 1d : 0d;
                    break;
            }
        }
    }

}
