using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;
using System.Collections.Generic;

namespace Game.Behaviors
{
    public class HealthBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_Health = "Health";
        public const string Key_Lifes = "Lifes";
        public const string Key_IsRespawning = "IsRespawning";
        public const string Key_RespawnTime = "RespawnTime";

        private float elapsedTime;
        private List<DamageEvent> damageReceivedSinceLastUpdate = new List<DamageEvent>();

        public HealthBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Health, new Attribute<int>(1));
            entity.AddAttribute(Key_Lifes, new Attribute<int>(1));
            entity.AddAttribute(Key_IsRespawning, new Attribute<bool>(false));
            entity.AddAttribute(Key_RespawnTime, new Attribute<float>(2f));

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(DamageEvent));
            handledEventTypes.Add(typeof(RespawnEntityEvent));
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach(DamageEvent evt in damageReceivedSinceLastUpdate) {            
                handleDamage(evt);
            }
            damageReceivedSinceLastUpdate.Clear();

            Attribute<bool> isRespawning = entity[Key_IsRespawning];
            if (isRespawning) 
            {
                elapsedTime += deltaTime;
                Attribute<float> respawnTime = entity[Key_RespawnTime];

                if (elapsedTime >= respawnTime) 
                {
                    respawn(ref isRespawning);
                }
            }

        }

        private void handleDamage(DamageEvent evt)
        {
            if (entity.IsDead) 
            {
                return;
            }

            Attribute<int> health = entity[Key_Health];
            health.Value -= evt.Damage;

            if (health <= 0) 
            {
                Attribute<int> lifes = entity[Key_Lifes];
                lifes.Value -= 1;
                entity.State = EntityState.Dying;

                if (entity.Type == "player")
                {
                    HudEvent hudEvent = HudEvent.UpdateLifes(lifes);
                    eventManager.QueueEvent(hudEvent);
                }

                if (lifes <= 0) 
                {
                    Entity projectile = entity.Game.World.Entities[evt.SourceEntityID];
                    Attribute<Entity> projectileOwner = projectile[ProjectileBehavior.Key_ProjectileOwner]
                        as Attribute<Entity>;

                    int destroyedByEntityID = projectile.ID;
                    if (projectileOwner != null) 
                    {
                        destroyedByEntityID = projectileOwner.Value.ID;
                    }

                    eventManager.QueueEvent(new DestroyEntityEvent(DestroyEntityEvent.DESTROY_ENTITY, entity.ID,
                        destroyedByEntityID));
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " died a horrible "
                        + "death!");
                }
                else {
                    RespawnEntityEvent respawnEvent = new RespawnEntityEvent(RespawnEntityEvent.RESPAWN_ENTITY,
                        entity.ID);
                    eventManager.QueueEvent(respawnEvent);
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " lost a life. " 
                        + lifes + " lifes remaining. Respawning...");
                }
            }
        }

        private void respawn(ref Attribute<bool> isRespawning)
        {
            Attribute<int> health = entity[Key_Health];
            health.Value = 1;
            entity.State = EntityState.Active;

            elapsedTime = 0f;
            isRespawning.Value = false;

            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position];
            position.Value.X = world.Width / 2f - (75f / 2f);
            position.Value.Y = 100 - (75f / 2f);

            entity.State = EntityState.Active;

            Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " respawned.");
        }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) 
            {
                case DamageEvent.RECEIVE_DAMAGE:
                {
                    if (entity.IsDead) 
                    {
                        break;
                    }
                    DamageEvent msg = (DamageEvent)evt;
                    damageReceivedSinceLastUpdate.Add(msg);

                    break;
                }
                case RespawnEntityEvent.RESPAWN_ENTITY: {
                    Attribute<bool> isRespawning = entity[Key_IsRespawning];
                    isRespawning.Value = true;
                    break;
                }
            }
        }
    }
}
