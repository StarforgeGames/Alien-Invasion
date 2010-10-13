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
        public const string Key_Lifes = "Lifes";

        public HealthBehaviour(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { typeof(DamageEvent), typeof(RespawnEntityEvent) };

            entity.AddAttribute(Key_Health, new Attribute<int>(1));
            entity.AddAttribute(Key_Lifes, new Attribute<int>(1));
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<int> health = entity[Key_Health] as Attribute<int>;

            if (health <= 0) {
                Attribute<int> lifes = entity[Key_Lifes] as Attribute<int>;
                lifes.Value -= 1;

                if (lifes.Value <= 0) {
                    entity.State = EntityState.Dead;
                    EventManager.QueueEvent(new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID));
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name
                        + " died a horrible death!");
                }
                else {
                    entity.State = EntityState.Inactive;
                    RespawnEntityEvent respawnEvent = new RespawnEntityEvent(RespawnEntityEvent.RESPAWN_ENTITY,
                        entity.ID);
                    EventManager.QueueEvent(respawnEvent);
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name
                        + " lost a life. " + lifes + " lifes remaining. Respawning...");
                }
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
                    break;
                }
                case RespawnEntityEvent.RESPAWN_ENTITY: {
                    Attribute<int> health = entity[Key_Health] as Attribute<int>;
                    health.Value = 1;
                    entity.State = EntityState.Active;

                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name + " respawned.");
                    break;
                }
            }
        }
    }
}
