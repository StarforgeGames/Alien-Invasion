using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.EventManagement.Events;
using Game.Entities;

namespace Game.Behaviours
{
    class HealthBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_Health = "Health";

        public HealthBehaviour(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { typeof(DamageEvent) };

            entity.AddAttribute(Key_Health, new Attribute<int>(1));
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<int> health = entity[Key_Health] as Attribute<int>;

            if (health <= 0) {
                entity.Kill();
                Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name + " died a horrible death!");
            }
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case DamageEvent.RECEIVE_DAMAGE: {
                    DamageEvent dmgMsg = (DamageEvent) evt;

                    Attribute<int> health = entity[Key_Health] as Attribute<int>;
                    health.Value -= dmgMsg.Damage;

                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name + " received " 
                        + dmgMsg.Damage + " damage " + "(Remaining HP:" + health + "). Ouch!");
                }
                break;
            }
        }
    }
}
