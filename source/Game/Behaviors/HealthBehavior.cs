﻿using System;
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
        public const string Key_RespawnTime = "RespawnTime";

        private float elapsedTime;
        private bool isRespawning;

        private List<DamageEvent> damageReceivedSinceLastUpdate = new List<DamageEvent>();

        public HealthBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_Health, 1);
            entity.AddAttribute(Key_Lifes, 1);
            entity.AddAttribute(Key_RespawnTime, 2.0f);

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

            if (isRespawning) 
            {
                elapsedTime += deltaTime;
                float respawnTime = entity[Key_RespawnTime];

                if (elapsedTime >= respawnTime) 
                {
                    respawn();
                }
            }

        }

        private void handleDamage(DamageEvent evt)
        {
            if (entity.IsDead) 
            {
                return;
            }

            int health = entity[Key_Health];
            health -= evt.Damage;
            entity[Key_Health] = health;

            if (health <= 0) 
            {
                int lifes = entity[Key_Lifes];
                lifes -= 1;
                entity[Key_Lifes] = lifes;
                entity.State = EntityState.Dying;

                if (entity.Type == "player")
                {
                    HudEvent hudEvent = HudEvent.UpdateLifes(lifes);
                    eventManager.QueueEvent(hudEvent);

                    CreateEntityEvent createEvt = CreateEntityEvent.New("player_death");
                    Vector2D position = entity[SpatialBehavior.Key_Position];
                    position.X -= 90;
                    position.Y -= 90;
                    createEvt.AddAttribute(SpatialBehavior.Key_Position, position);
                    eventManager.QueueEvent(createEvt);

                    entity[RenderBehavior.Key_IsRenderable] = false;
                }

                if (lifes <= 0) 
                {
                    Entity projectile = entity.Game.World.Entities[evt.SourceEntityID];
                    Entity projectileOwner = projectile[ProjectileBehavior.Key_ProjectileOwner];

                    int destroyedByEntityID = projectile.ID;
                    if (projectileOwner != null) 
                    {
                        destroyedByEntityID = projectileOwner.ID;
                    }

                    eventManager.QueueEvent(DestroyEntityEvent.Destroy(entity.ID, destroyedByEntityID));
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " died a horrible "
                        + "death!");
                }
                else {
                    RespawnEntityEvent respawnEvent = RespawnEntityEvent.Respawn(entity.ID);
                    eventManager.QueueEvent(respawnEvent);
                    Console.WriteLine("[" + this.GetType().Name + "] Entity " + entity.Type + " lost a life. " 
                        + lifes + " lifes remaining. Respawning...");
                }
            }
        }

        private void respawn()
        {
            entity[RenderBehavior.Key_IsRenderable] = true;
            entity[Key_Health] = 1;
            entity.State = EntityState.Active;

            elapsedTime = 0f;
            isRespawning = false;

            Vector2D position = entity[SpatialBehavior.Key_Position];
            position.X = world.Width / 2f - (75f / 2f);
            position.Y = 75 - (75f / 2f);
            entity[SpatialBehavior.Key_Position] = position;

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
                    isRespawning = true;
                    break;
                }
            }
        }
    }
}
