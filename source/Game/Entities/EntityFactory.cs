using System;
using System.Collections.Generic;
using Game.Entities.AttributeLoader;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game.Entities.AttributeParser;

namespace Game.Entities
{

    public class EntityFactory
    {
        private GameLogic game;
        private IEventManager eventManager;
        private IAttributeLoader attributeLoader = new DefaultAttributeLoader();

        public EntityFactory(GameLogic game)
        {
            this.game = game;
            this.eventManager = game.EventManager;
        }

        public Entity New(string id, Dictionary<string, object> customAttributes = null)
        {
            Entity entity = new Entity(game, id);
            entity.Load(attributeLoader, @"data\entities\" + id + ".xml");

            Console.WriteLine("[" + this.GetType().Name + "] Created entity " + entity);

            if (customAttributes != null) {
                foreach (KeyValuePair<string, object> pair in customAttributes) {
                    entity[pair.Key] = pair.Value;
                }
            }

            NewEntityEvent newEntityEvent = new NewEntityEvent(NewEntityEvent.NEW_ENTITY, entity.ID);
            eventManager.QueueEvent(newEntityEvent);

            return entity;
        }

        public void Add(IAttributeParser parser)
        {
            attributeLoader.Add(parser);
        }

        public void Remove(string type)
        {
            attributeLoader.Remove(type);
        }
    }

}
