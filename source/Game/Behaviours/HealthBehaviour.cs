﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Messages;
using Game.Entities;

namespace Game.Behaviours
{
    class HealthBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_Health = "Health";

        public HealthBehaviour(Entity entity, int health)
            : base(entity)
        {
            entity.AddAttribute(Key_Health, new Attribute<int>(health));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() {
            typeof(DamageMessage)
        };
        public override ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public override void OnUpdate(float deltaTime)
        {
        }

        public override void OnMessage(Message msg)
        {
            switch (msg.Type) {
                case DamageMessage.DEAL_DAMAGE: {
                    DamageMessage dmgMsg = (DamageMessage) msg;

                    Attribute<int> health = entity[Key_Health] as Attribute<int>;
                    health.Value -= dmgMsg.Damage;

                    if (health <= 0) {
                        entity.State = EntityState.Dead;

                        Message deathMsg = new DeathMessage(DeathMessage.ACTOR_DIES);
                        entity.SendBroadcastMessage(deathMsg);

                        Console.WriteLine("Entity " + entity.Name + " died a horrible death!");
                    }

                    Console.WriteLine("Entity " + entity.Name + " received " + dmgMsg.Damage + " damage. Ouch!");
                }
                break;
            }
        }

        #endregion
    }
}
