using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Messages
{

    class DamageMessage : Message
    {
        // Event Message Types
        public const string DEAL_DAMAGE = "actor_deal_damage";

        public int Damage { get; set; }

        public DamageMessage(string type, int damage)
        {
            this.Type = type;
            this.Damage = damage > 0 ? damage : 0;
        }
    }

}
