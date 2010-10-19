﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;
using Game.Behaviors;
using Game.EventManagement;
using Game.EventManagement.Events;

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

        public string Type { get; private set; }
        public GameLogic Game { get; private set; }

        public IEventManager EventManager { get; private set; }
        private Dictionary<Type, List<IEventListener>> listenerMap;

        private static int nextEntityId = 1;
        private readonly int id;
        public int ID { get { return id; } }

        private Dictionary<string, object> attributes;
        private List<IBehavior> behaviors;

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

        public Entity(GameLogic game, string type)
        {
            State = EntityState.Active;

            this.Type = type; 
            this.Game = game;

            EventManager = game.EventManager;
            listenerMap = new Dictionary<Type, List<IEventListener>>();

            id = nextEntityId++;

            behaviors = new List<IBehavior>();
            attributes = new Dictionary<string, object>();
        }

        public void Load(string xmlFile)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(xmlFile);

            XmlNode behaviorNode = xml.GetElementsByTagName("behaviors")[0];
            foreach (XmlNode node in behaviorNode.ChildNodes) {
                Type t = System.Type.GetType(typeof(IBehavior).Namespace + "." + node.Name);
                object[] param = new object[] { this };
                IBehavior b = Activator.CreateInstance(t, param) as IBehavior;

                AddBehavior(b);
            }
            
            XmlNode attributeNode = xml.GetElementsByTagName("attributes")[0];
            foreach (XmlNode node in attributeNode.ChildNodes) {
                object attribute = this[node.Attributes["key"].Value];
                PropertyInfo valueProp = attribute.GetType().GetProperty("Value");
                object value = extractValue(node);

                valueProp.SetValue(attribute, value, null);
            }
        }

        private object extractValue(XmlNode node)
        {
            switch (node.Name) {
                case "bool":
                    bool b = bool.Parse(node.InnerText);
                    return b;
                case "string":
                    return node.InnerText;
                case "int":
                    int i = int.Parse(node.InnerText);
                    return i;
                case "float":
                    float f = float.Parse(node.InnerText, NumberStyles.Number,  
                        CultureInfo.InvariantCulture.NumberFormat);
                    return f;
                case "double":
                    double d = double.Parse(node.InnerText, NumberStyles.Number,
                        CultureInfo.InvariantCulture.NumberFormat);
                    return d;
                default:
                    return AttributeLoader.Load(node);
            }
        }

        /// <summary>
        /// Adds a new behavior, registering it as listener to all supported messages as well.
        /// </summary>
        /// <param name="behavior">The behavior to add</param>
        public void AddBehavior(IBehavior behavior)
        {
            behaviors.Add(behavior);

            foreach (Type type in behavior.HandledEventTypes) {
                if (!listenerMap.ContainsKey(type)) {
                    listenerMap.Add(type, new List<IEventListener>());
                }

                listenerMap[type].Add(behavior);
                EventManager.AddListener(this, type);
            }
        }

        /// <summary>
        /// Removes a behavior and unregisters it as listener to events.
        /// </summary>
        /// <param name="behavior">The behavior to remove</param>
        /// <returns>True if removal was successful</returns>
        public bool RemoveBehavior(IBehavior behavior)
        {
            foreach (Type type in behavior.HandledEventTypes) {
                listenerMap[type].Remove(behavior);
                EventManager.RemoveListener(this, type);
            }

            return behaviors.Remove(behavior);
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

            foreach (IBehavior b in behaviors) {
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
            return Type + " (#" + ID + ")";
        }
    }

}
