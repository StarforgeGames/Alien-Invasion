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
        public const string Key_IsRespawning = "IsRespawning";
        public const string Key_RespawnTime = "RespawnTime";

        private float elapsedTime = 0f;

        public HealthBehaviour(Entity entity)
            : base(entity)
        {
            handledEventTypes = new List<Type>() { typeof(DamageEvent), typeof(RespawnEntityEvent) };

            entity.AddAttribute(Key_Health, new Attribute<int>(1));
            entity.AddAttribute(Key_Lifes, new Attribute<int>(1));
            entity.AddAttribute(Key_IsRespawning, new Attribute<bool>(false));
            entity.AddAttribute(Key_RespawnTime, new Attribute<float>(2f));
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<int> lifes = entity[Key_Lifes] as Attribute<int>;
            if (lifes <= 0) {
                entity.State = EntityState.Dead;
            }

            Attribute<bool> isRespawning = entity[Key_IsRespawning] as Attribute<bool>;
            if (isRespawning) {
                entity.State = EntityState.Inactive;

                elapsedTime += deltaTime;
                Attribute<float> respawnTime = entity[Key_RespawnTime] as Attribute<float>;

                if (elapsedTime >= respawnTime) {
                    Attribute<int> health = entity[Key_Health] as Attribute<int>;
                    health.Value = 1;
                    entity.State = EntityState.Active;

                    elapsedTime = 0f;
                    isRespawning.Value = false;

                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name + " respawned.");
                }
            }

        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case DamageEvent.RECEIVE_DAMAGE: {
                    DamageEvent dmgMsg = (DamageEvent) evt;
                    applyDamage(dmgMsg.Damage);
                    break;
                }
                case RespawnEntityEvent.RESPAWN_ENTITY: {
                    Attribute<bool> isRespawning = entity[Key_IsRespawning] as Attribute<bool>;
                    isRespawning.Value = true;
                    break;
                }
            }
        }

        private void applyDamage(int damage)
        {
            Attribute<int> health = entity[Key_Health] as Attribute<int>;
            health.Value -= damage;

            if (health <= 0) {
                Attribute<int> lifes = entity[Key_Lifes] as Attribute<int>;
                lifes.Value -= 1;

                if (lifes <= 0) {
                    EventManager.QueueEvent(new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID));
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name
                        + " died a horrible death!");
                }
                else {
                    RespawnEntityEvent respawnEvent = new RespawnEntityEvent(RespawnEntityEvent.RESPAWN_ENTITY,
                        entity.ID);
                    EventManager.QueueEvent(respawnEvent);
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Name
                        + " lost a life. " + lifes + " lifes remaining. Respawning...");
                }
            }
        }
    }
}
