using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.Messages;

namespace Game.Behaviours
{

    class SpatialBehaviour : IBehaviour
    {
        List<Type> supportedMessages = new List<Type>() {
            typeof(MoveMessage)
        };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        // Attribute Keys
        public readonly int Key_PositionX;
        public readonly int Key_PositionY;
        public readonly int Key_Speed;

        private Entity entity;
        
        private Direction direction;
        private bool isMoving;
        
        public SpatialBehaviour(Entity entity, float posX, float posY, float speed)
        {
            this.entity = entity;

            Key_PositionX = entity.AddAttribute(new Attribute<float>(posX));
            Key_PositionY = entity.AddAttribute(new Attribute<float>(posY));
            Key_Speed = entity.AddAttribute(new Attribute<float>(speed));
        }

        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {            
            if (isMoving) {
                Attribute<float> speed = entity[Key_Speed] as Attribute<float>;
                Attribute<float> posX = entity[Key_PositionX] as Attribute<float>;
                Attribute<float> posY = entity[Key_PositionY] as Attribute<float>;

                switch (direction) {
                    case Direction.North: {
                        posY.Value += speed * deltaTime;
                        break;
                    }
                    case Direction.East: {
                        posX.Value += speed * deltaTime;
                        break;
                    }
                    case Direction.South: {
                        posY.Value -= speed * deltaTime;
                        break;
                    }
                    case Direction.West: {
                        posX.Value -= speed * deltaTime;
                        break;
                    }
                }

                Attribute<float> x = (Attribute<float>)entity.GetAttribute(Key_PositionX);
                Attribute<float> y = (Attribute<float>)entity.GetAttribute(Key_PositionY);
                Console.WriteLine("Moved " + direction.ToString() + " to (" + x.Value + "/" + y.Value + ")");
            }
        }

        public void OnMessage(Message msg)
        {
            switch (msg.Type) {
                case MoveMessage.START_MOVING:
                    if (msg is MoveMessage) {
                        MoveMessage moveMsg = (MoveMessage)msg;

                        this.isMoving = true;
                        this.direction = moveMsg.Direction;
                    }
                    break;
                case MoveMessage.STOP_MOVING:
                    isMoving = false;
                    break;
            }
        }

        #endregion
    }

}
