using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;

namespace Game.Messages
{
    class CollisionMessage : Message
    {
        // Event Message Types
        public const string ACTOR_COLLIDES = "actor_collides";

        public Entity OtherEntity { get; set; }

        public CollisionMessage(string type, Entity otherEntity)
            : base(type)
        {
            this.OtherEntity = otherEntity;
        }
    }
}
