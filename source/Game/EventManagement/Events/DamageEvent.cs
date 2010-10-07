using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;

namespace Game.EventManagement.Events
{

    class DamageEvent : Event
    {
        // Event Message Types
        public const string RECEIVE_DAMAGE = "actor_receive_damage";

        public int Damage { get; set; }
        public Entity Source { get; set; }

        public DamageEvent(string type, int damage, Entity source)
            : base(type)
        {
            this.Damage = damage > 0 ? damage : 0;
            this.Source = source;
        }
    }

}
