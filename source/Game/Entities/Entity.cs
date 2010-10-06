using System.Collections.Generic;
using Game.Behaviours;
using Game.Messages;
using System;

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

        private Dictionary<string, object> attributes;
        private List<IBehaviour> behaviours;
        private Dictionary<Type, List<IBehaviour>> observers;

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

            behaviours = new List<IBehaviour>();
            attributes = new Dictionary<string, object>();
            observers = new Dictionary<Type, List<IBehaviour>>();
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
                if (!observers.ContainsKey(type)) {
                    observers.Add(type, new List<IBehaviour>());
                }
                observers[type].Add(behaviour);
            }
        }

        /// <summary>
        /// Removes a behaviour and unregisters it as listener to events.
        /// </summary>
        /// <param name="behaviour">The behaviour to remove</param>
        /// <returns>True if removal was successful</returns>
        public bool RemoveBehaviour(IBehaviour behaviour)
        {
            foreach (List<IBehaviour> list in observers.Values) {
                foreach (IBehaviour b in list) {
                    list.Remove(behaviour);
                }
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

        /// <summary>
        /// Sends a message to all behaviours who listen to those kind of message.
        /// </summary>
        /// <param name="msg">Message</param>
        public void SendMessage(Message msg)
        {
            Type t = msg.GetType();

            if (observers.ContainsKey(t)) {
                foreach (IBehaviour b in observers[t]) {
                    b.OnMessage(msg);
                }
            }
        }

        /// <summary>
        /// Sends a message to all behaviours, whether they are registered to listen to this kind of message or not.
        /// </summary>
        /// <param name="msg">Global message</param>
        public void SendBroadcastMessage(Message msg)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnMessage(msg);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }

        public void Kill()
        {
            this.State = EntityState.Dead;

            Message deathMsg = new DeathMessage(DeathMessage.ACTOR_DIES);
            SendBroadcastMessage(deathMsg);
        }
    }

}
