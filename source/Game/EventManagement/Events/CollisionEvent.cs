using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;

namespace Game.EventManagement.Events
{
    class CollisionEvent : Event
    {
        // Event Message Types
        public const string ACTOR_COLLIDES = "actor_collides";

        public Entity OtherEntity { get; set; }

        public CollisionEvent(string type, Entity otherEntity)
            : base(type)
        {
            this.OtherEntity = otherEntity;
        }
    }
}
