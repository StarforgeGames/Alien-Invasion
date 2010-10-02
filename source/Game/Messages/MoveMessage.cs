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
        // Event Message Types
        public const string START_MOVING = "actor_start_moving";
        public const string STOP_MOVING = "actor_stop_moving";

        public Direction Direction { get; set; }

        public MoveMessage() {
            Type = STOP_MOVING;
        }

        public MoveMessage(Direction direction)
        {
            Direction = direction;
            Type = START_MOVING;
        }
    }

}
