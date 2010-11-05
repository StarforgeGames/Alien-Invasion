using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    class HealthBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Health = "Health";
        public const string Key_Lifes = "Lifes";
        public const string Key_IsRespawning = "IsRespawning";
        public const string Key_RespawnTime = "RespawnTime";

        private float elapsedTime;
        private int damageTaken;

        public HealthBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Health, new Attribute<int>(1));
            entity.AddAttribute(Key_Lifes, new Attribute<int>(1));
            entity.AddAttribute(Key_IsRespawning, new Attribute<bool>(false));
            entity.AddAttribute(Key_RespawnTime, new Attribute<float>(2f));
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(DamageEvent));
            handledEventTypes.Add(typeof(RespawnEntityEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            if (damageTaken > 0) {
                applyDamage(damageTaken);
                damageTaken = 0;
            }

            Attribute<bool> isRespawning = entity[Key_IsRespawning] as Attribute<bool>;
            if (isRespawning) {
                elapsedTime += deltaTime;
                Attribute<float> respawnTime = entity[Key_RespawnTime] as Attribute<float>;

                if (elapsedTime >= respawnTime) {
                    respawn(ref isRespawning);
                }
            }

        }

        private void applyDamage(int damage)
        {
            if (entity.IsDead) {
                return;
            }

            Attribute<int> health = entity[Key_Health] as Attribute<int>;
            health.Value -= damage;

            if (health <= 0) {
                Attribute<int> lifes = entity[Key_Lifes] as Attribute<int>;
                lifes.Value -= 1;
                entity.State = EntityState.Dead;

                if (lifes <= 0) {
                    EventManager.QueueEvent(new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID));
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type
                        + " died a horrible death!");
                }
                else {
                    RespawnEntityEvent respawnEvent = new RespawnEntityEvent(RespawnEntityEvent.RESPAWN_ENTITY,
                        entity.ID);
                    EventManager.QueueEvent(respawnEvent);
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type
                        + " lost a life. " + lifes + " lifes remaining. Respawning...");
                }
            }
        }

        private void respawn(ref Attribute<bool> isRespawning)
        {
            Attribute<int> health = entity[Key_Health] as Attribute<int>;
            health.Value = 1;
            entity.State = EntityState.Active;

            elapsedTime = 0f;
            isRespawning.Value = false;

            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            position.Value.X = entity.Game.WorldWidth / 2f - (75f / 2f);
            position.Value.Y = 100 - (75f / 2f);

            entity.State = EntityState.Active;

            Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " respawned.");
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case DamageEvent.RECEIVE_DAMAGE: {
                    DamageEvent dmgMsg = (DamageEvent) evt;
                    damageTaken += dmgMsg.Damage;
                    break;
                }
                case RespawnEntityEvent.RESPAWN_ENTITY: {
                    Attribute<bool> isRespawning = entity[Key_IsRespawning] as Attribute<bool>;
                    isRespawning.Value = true;
                    break;
                }
            }
        }
    }
}
