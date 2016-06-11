using System;
using System.Collections.Generic;
using Game.Entities.AttributeLoader;
using Game.EventManagement;

namespace Game.Entities
{

    public class EntityFactory
    {
        private GameLogic game;
        private IEventManager eventManager;
        private IAttributeLoader attributeLoader;

        public EntityFactory(GameLogic game)
        {
            this.game = game;
            this.eventManager = game.EventManager;
            attributeLoader = new DefaultAttributeLoader(game.ResourceManager);
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

            return entity;
        }
    }

}
