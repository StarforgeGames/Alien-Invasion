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
        Inactive,
        Dead
    }

    public class Entity : IEventListener
    {
        public EntityState State { get; set; }

        public string Name { get; private set; }
        public GameLogic Game { get; private set; }

        public IEventManager EventManager { get; private set; }
        private Dictionary<Type, List<IEventListener>> listenerMap;

        private static int nextEntityId = 1;
        private readonly int id;
        public int ID { get { return id; } }

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

        public Entity(GameLogic game, string name)
        {
            State = EntityState.Active;

            // that's the
            this.Name = name; 
            // of the
            this.Game = game;

            EventManager = game.EventManager;
            listenerMap = new Dictionary<Type, List<IEventListener>>();

            id = nextEntityId++;

            behaviours = new List<IBehaviour>();
            attributes = new Dictionary<string, object>();
        }

        /// <summary>
        /// Adds a new behaviour, registering it as listener to all supported messages as well.
        /// </summary>
        /// <param name="behaviour">The behaviour to add</param>
        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviours.Add(behaviour);

            foreach (Type type in behaviour.HandledEventTypes) {
                if (!listenerMap.ContainsKey(type)) {
                    listenerMap.Add(type, new List<IEventListener>());
                }

                listenerMap[type].Add(behaviour);
                EventManager.AddListener(this, type);
            }
        }

        /// <summary>
        /// Removes a behaviour and unregisters it as listener to events.
        /// </summary>
        /// <param name="behaviour">The behaviour to remove</param>
        /// <returns>True if removal was successful</returns>
        public bool RemoveBehaviour(IBehaviour behaviour)
        {
            foreach (Type type in behaviour.HandledEventTypes) {
                listenerMap[type].Remove(behaviour);
                EventManager.RemoveListener(this, type);
            }

            return behaviours.Remove(behaviour);
        }

        /// <summary>
        /// Adds a new attribute to this entity.
        /// </summary>
        /// <param name="value">Value of the new attribute</param>
        /// <returns>Unique identifier for this attribute</returns>
        public void AddAttribute(string key, object value)
        {
            attributes.Add(key, value);
        }

        public void Update(float deltaTime)
        {
            EventManager.Tick();

            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }

        public void OnEvent(Event evt)
        {
            if(!listenerMap.ContainsKey(evt.GetType())) {
                return;
            }

            foreach (IEventListener listener in listenerMap[evt.GetType()]) {
                listener.OnEvent(evt);
            }
        }

        public override string ToString()
        {
            return Name + " (#" + ID + ")";
        }
    }

}
