using System.Collections.Generic;
using Game.Behaviours;
using Game.EventManagement.Events;
using System;
using Game.EventManagement;

namespace Game.Entities
{
    public enum EntityState
    {
        Active,
        Paused,
        Dead
    }

    public class Entity
    {
        public EntityState State { get; set; }

        public string Name { get; private set; }
        public BaseGame Game { get; private set; }

        public IEventManager EventManager { get; private set; }

        private Dictionary<string, object> attributes;
        private List<IBehaviour> behaviours;

        public object this[string key]
        { 
            get { 
                if(attributes.ContainsKey(key)) {
                    return attributes[key]; 
                }
                return null;
            }
            set { attributes[key] = value;  } 
        }

        public Entity(BaseGame game, string name)
        {
            State = EntityState.Active;

            // that's the
            this.Name = name; 
            // of the
            this.Game = game;

            EventManager = new SwappingEventManager();

            behaviours = new List<IBehaviour>();
            attributes = new Dictionary<string, object>();
        }


        #region Behaviours

        /// <summary>
        /// Adds a new behaviour, registering it as listener to all supported messages as well.
        /// </summary>
        /// <param name="behaviour">The behaviour to add</param>
        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviours.Add(behaviour);

            foreach (Type type in behaviour.SupportedMessages) {
                EventManager.AddListener(behaviour, type);
            }
        }

        /// <summary>
        /// Removes a behaviour and unregisters it as listener to events.
        /// </summary>
        /// <param name="behaviour">The behaviour to remove</param>
        /// <returns>True if removal was successful</returns>
        public bool RemoveBehaviour(IBehaviour behaviour)
        {
            foreach (Type type in behaviour.SupportedMessages) {
                EventManager.RemoveListener(behaviour, type);
            }

            return behaviours.Remove(behaviour);
        }

        #endregion

        #region Attributes

        /// <summary>
        /// Adds a new attribute to this entity.
        /// </summary>
        /// <param name="value">Value of the new attribute</param>
        /// <returns>Unique identifier for this attribute</returns>
        public void AddAttribute(string key, object value)
        {
            attributes.Add(key, value);
        }

        #endregion

        public void Update(float deltaTime)
        {
            EventManager.Tick();

            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }

        public void Kill()
        {
            this.State = EntityState.Dead;
            EventManager.QueueEvent(new DeathEvent(DeathEvent.ACTOR_DIES));
        }
    }

}
