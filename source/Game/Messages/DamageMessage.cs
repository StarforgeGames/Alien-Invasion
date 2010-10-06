using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Messages
{

    class DamageMessage : Message
    {
        // Event Message Types
        public const string RECEIVE_DAMAGE = "actor_receive_damage";

        public int Damage { get; set; }

        public DamageMessage(string type, int damage)
            : base(type)
        {
            this.Damage = damage > 0 ? damage : 0;
        }
    }

}
