using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.EventManagement.Events
{

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    class MoveEvent : Event
    {
        // Event Message Types
        public const string START_MOVING = "actor_start_moving";
        public const string STOP_MOVING = "actor_stop_moving";

        public Direction Direction { get; set; }

        public MoveEvent(string type, Direction direction)
            : base(type)
        {
            this.Direction = direction;
        }
    }

}
