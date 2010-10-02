using System.Collections.Generic;
using Game.Behaviours;
using Game.Messages;
using System;

namespace Game.Entities
{

    public class Entity
    {
        private Dictionary<int, IAttribute> attributes;
        private List<IBehaviour> behaviours;
        private Dictionary<Type, List<IBehaviour>> messageObservers;

        private int nextAttributeID = 0;
        public int NextAttributeID {
            get { return nextAttributeID++; } 
        }

        protected BaseGame game;
        public BaseGame Game { get { return game; } }

        public Entity(BaseGame game)
        {
            behaviours = new List<IBehaviour>();
            attributes = new Dictionary<int, IAttribute>();
            messageObservers = new Dictionary<Type, List<IBehaviour>>();
            this.game = game;
        }


        #region Behaviours

        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviours.Add(behaviour);

            foreach (Type type in behaviour.SupportedMessages) {
                if (!messageObservers.ContainsKey(type)) {
                    messageObservers.Add(type, new List<IBehaviour>());
                }
                messageObservers[type].Add(behaviour);
            }
        }

        public bool RemoveBehaviour(IBehaviour behaviour)
        {
            foreach (List<IBehaviour> list in messageObservers.Values) {
                foreach (IBehaviour b in list) {
                    list.Remove(behaviour);
                }
            }

            return behaviours.Remove(behaviour);
        }

        #endregion


        #region Attributes

        public void AddAttribute(int key, int value)
        {
            attributes.Add(key, new Attribute<int>(value));
        }

        public void AddAttribute(int key, float value)
        {
            attributes.Add(key, new Attribute<float>(value));
        }

        public void AddAttribute(int key, bool value)
        {
            attributes.Add(key, new Attribute<bool>(value));
        }

        public void AddAttribute(int key, string value)
        {
            attributes.Add(key, new Attribute<string>(value));
        }

        public IAttribute GetAttribute(int key)
        {
            return attributes[key];
        }

        #endregion


        public void SendMessage(Message msg)
        {
            Type t = msg.GetType();

            if (messageObservers.ContainsKey(t)) {
                foreach (IBehaviour b in messageObservers[t]) {
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
