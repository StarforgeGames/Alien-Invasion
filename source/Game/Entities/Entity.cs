using System.Collections.Generic;
using Game.Behaviours;
using Game.Messages;
using System;

namespace Game.Entities
{

    public class Entity
    {
        private Dictionary<int, object> attributes;
        private List<IBehaviour> behaviours;
        private Dictionary<Type, List<IBehaviour>> observers;

        private int nextAttributeID = 0;

        protected BaseGame game;
        public BaseGame Game { get { return game; } }

        public object this[int key] { 
            get { return attributes[key]; }
            set { attributes[key] = value;  } 
        }

        public Entity(BaseGame game)
        {
            behaviours = new List<IBehaviour>();
            attributes = new Dictionary<int, object>();
            observers = new Dictionary<Type, List<IBehaviour>>();
            this.game = game;
        }


        #region Behaviours

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

        public int AddAttribute(object value)
        {
            int key = nextAttributeID++;
            attributes.Add(key, value);

            return key;
        }

        public object GetAttribute(int key)
        {
            return attributes[key];
        }

        #endregion

        public void SendMessage(Message msg)
        {
            Type t = msg.GetType();

            if (observers.ContainsKey(t)) {
                foreach (IBehaviour b in observers[t]) {
                    b.OnMessage(msg);
                }
            }
        }

        public void Update(float deltaTime)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }
    }

}
