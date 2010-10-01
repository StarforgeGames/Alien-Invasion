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

        // Message Types
        public const int START_MOVING = 1;
        public const int STOP_MOVING = 2;

        // Attribute Keys
        public readonly int Key_PositionX;
        public readonly int Key_PositionY;

        private Entity entity;
        
        private Direction direction;
        private bool isMoving;

        private float speed;

        public SpatialBehaviour(Entity entity, float speed, float posX, float posY)
        {
            this.entity = entity;
            this.speed = speed;

            Key_PositionX = entity.NextAttributeID;
            entity.AddAttribute(Key_PositionX, posX);

            Key_PositionY = entity.NextAttributeID;
            entity.AddAttribute(Key_PositionY, posY);
        }

        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {            
            if (isMoving) {
                float timeInSeconds = deltaTime / 1000;

                switch (direction) {
                    case Direction.North: {
                        Attribute<float> posY = (Attribute<float>)entity.GetAttribute(Key_PositionY);
                        posY.Value += (speed * timeInSeconds);
                        break;
                    }
                    case Direction.East: {
                        Attribute<float> posX = (Attribute<float>)entity.GetAttribute(Key_PositionX);
                        posX.Value += (speed * timeInSeconds);
                        break;
                    }
                    case Direction.South: {
                        Attribute<float> posY = (Attribute<float>)entity.GetAttribute(Key_PositionY);
                        posY.Value -= (speed * timeInSeconds);
                        break;
                    }
                    case Direction.West: {
                        Attribute<float> posX = (Attribute<float>)entity.GetAttribute(Key_PositionX);
                        posX.Value -= (speed * timeInSeconds);
                        break;
                    }
                }
            }
        }

        public void OnMessage(Message msg)
        {
            switch (msg.Type) {
                case START_MOVING:
                    if (msg is MoveMessage) {
                        MoveMessage moveMsg = (MoveMessage)msg;

                        this.isMoving = true;
                        this.direction = moveMsg.Direction;
                    }
                    break;
                case STOP_MOVING:
                    isMoving = false;
                    break;
            }
        }

        #endregion
    }

}
