using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviors;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Entities
{

    public class EntityFactory
    {
        private GameLogic game;
        private AttributeLoader loader = new AttributeLoader();

        public EntityFactory(GameLogic game)
        {
            this.game = game;
        }

        public Entity New(string id, Dictionary<string, object> customAttributes = null)
        {
            Entity entity = new Entity(game, id);
            entity.Load(loader, @"data\entities\" + id + ".xml");

            Console.WriteLine("[" + this.GetType().Name + "] Created entity " + entity);

            if (customAttributes != null) {
                foreach (KeyValuePair<string, object> pair in customAttributes) {
                    entity[pair.Key] = pair.Value;
                }
            }

            NewEntityEvent newEntityEvent = new NewEntityEvent(NewEntityEvent.NEW_ENTITY, entity.ID);
            game.EventManager.QueueEvent(newEntityEvent);

            return entity;
        }
    }

}
