using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.Behaviors;
using Game.EventManagement.Events;
using Game.EventManagement;
using Game.Utility;
using System.Diagnostics;
using Logging;

namespace Game
{
    public class CollisionDetector : IEventListener
    {
        public GameLogic Game { get; private set; }
        public IEventManager EventManager { get; private set; }

        private List<Entity> collidables;


        public CollisionDetector(GameLogic game)
        {
            this.Game = game;
            this.EventManager = game.EventManager;

            this.collidables = new List<Entity>();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(NewEntityEvent));
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
        }

        public void CheckForCollisionsInWorld()
        {
            foreach (Entity entity in Game.Entities.Values) {
                if (entity.IsDead) {
                    continue;
                }

                foreach (Entity other in Game.Entities.Values) {
                    if (entity.ID == other.ID || other.IsDead) {
                        continue;
                    }

                    Attribute<Entity> owner = entity[ProjectileBehavior.Key_ProjectileOwner] as Attribute<Entity>;
                    Attribute<Entity> otherOwner = other[ProjectileBehavior.Key_ProjectileOwner] as Attribute<Entity>;
                    if ((owner != null && owner.Value == other) || (otherOwner != null && otherOwner.Value == entity)) {
                        continue;
                    }

                    if (AreColliding(entity, other)) {
                        Console.WriteLine("[" + this.GetType().Name + "] " + entity.Type + " collided with " + other.Type
                            + "!");

                        CollisionEvent collisionMsg = new CollisionEvent(
                            CollisionEvent.ACTOR_COLLIDES,
                            other.ID,
                            entity.ID);
                        EventManager.QueueEvent(collisionMsg);

                        Attribute<int> collisionDmg = entity[CollisionBehavior.Key_CollisionDamage] as Attribute<int>;
                        DamageEvent dmgMsg = new DamageEvent(DamageEvent.RECEIVE_DAMAGE,
                            entity.ID,
                            collisionDmg,
                            other.ID);
                        EventManager.QueueEvent(dmgMsg);
                    }
                }
            }
            
        }
        

        public bool AreColliding(Entity entity, Entity other)
        {
            Attribute<Vector2D> position = entity[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            Attribute<Vector2D> dimensions = entity[SpatialBehavior.Key_Dimensions] as Attribute<Vector2D>;

            Attribute<Vector2D> otherPosition = other[SpatialBehavior.Key_Position] as Attribute<Vector2D>;
            Attribute<Vector2D> otherDimensions = other[SpatialBehavior.Key_Dimensions] as Attribute<Vector2D>;

            return position.Value.X <= otherPosition.Value.X + otherDimensions.Value.X
                && position.Value.Y <= otherPosition.Value.Y + otherDimensions.Value.Y
                && position.Value.X + dimensions.Value.X >= otherPosition.Value.X
                && position.Value.Y + dimensions.Value.Y >= otherPosition.Value.Y;
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY: {
                        NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                        Entity entity = Game.Entities[newEntityEvent.EntityID];
                        OnAttach(entity);
                        break;
                    }
                case DestroyEntityEvent.DESTROY_ENTITY: {
                        DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
                        Entity entity = Game.Entities[destroyEntityEvent.EntityID];
                        OnDetach(entity);
                        break;
                    }
            }
        }

        public void OnAttach(Entity entity)
        {
            Attribute<bool> isPhysical = entity[CollisionBehavior.Key_IsPhysical] as Attribute<bool>;
            if (isPhysical == null || !isPhysical) {
                return;
            }

            Log.Error("Adding " + entity.ToString() + "to collidables.", this.GetType().Name);
            collidables.Add(entity);
        }

        public void OnDetach(Entity entity)
        {
            if (collidables.Contains(entity)) {
                Log.Error("Removing " + entity.ToString() + "from collidables.", this.GetType().Name);
                collidables.Remove(entity);
            }
        }
    }
}
