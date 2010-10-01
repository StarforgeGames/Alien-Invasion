using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Messages
{

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    class MoveMessage : Message
    {
        public Direction Direction { get; set; }

        public MoveMessage() {
            Type = SpatialBehaviour.STOP_MOVING;
        }

        public MoveMessage(Direction direction)
        {
            Direction = direction;
            Type = SpatialBehaviour.START_MOVING;
        }
    }

}
