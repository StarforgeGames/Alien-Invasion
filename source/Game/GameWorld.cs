using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.EventManagement.Events;
using Game.EventManagement;
using Game.Utility;
using Game.Behaviors;

namespace Game
{
    public class GameWorld : IEventListener
    {
        public GameLogic Game { get; private set; }
        public IEventManager EventManager { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public EntityFactory EntityFactory { get; private set; }
        public Dictionary<int, Entity> Entities { get; private set; }

        private List<Entity> entitiesToRemove = new List<Entity>();
        private List<Entity> entitiesToAdd = new List<Entity>();

        public GameWorld(GameLogic gameLogic, int worldWidth, int worldHeight)
        {
            this.Game = gameLogic;
            this.EventManager = gameLogic.EventManager;

			this.Width = worldWidth;
			this.Height = worldHeight;

            EntityFactory = new EntityFactory(gameLogic);
            Entities = new Dictionary<int, Entity>();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(CreateEntityEvent));
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
        }

        public void Update(float deltaTime)
        {
            addNewEntities();
            destroyEntities();

            foreach (Entity entity in Entities.Values) {
                entity.Update(deltaTime);
            }
        }

        private void addNewEntities()
        {
            foreach (Entity entity in entitiesToAdd) {
                Entities.Add(entity.ID, entity);

                NewEntityEvent newEntityEvent = new NewEntityEvent(NewEntityEvent.NEW_ENTITY, entity.ID);
                EventManager.QueueEvent(newEntityEvent);

                System.Console.WriteLine("[" + this.GetType().Name + "] Added entity " + entity);
            }

            entitiesToAdd.Clear();
        }

        private void destroyEntities()
        {
            foreach (Entity entity in entitiesToRemove) {
                Entities.Remove(entity.ID);
                System.Console.WriteLine("[" + this.GetType().Name + "] Destroyed entity #" + entity.ID);
            }

            entitiesToRemove.Clear();
        }

        public void Initialize()
        {
            createAndInitializePlayer();
            createAndInitializeAliens();
        }

        public void Reset()
        {
            entitiesToAdd.Clear();
            entitiesToRemove.Clear();

            Entities.Clear();
        }

        private void createAndInitializePlayer()
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "player");

            float startX = Width / 2f - (75f / 2f);
            float startY = 50;
            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);
            Attribute<Vector2D> dimensions = new Attribute<Vector2D>(new Vector2D(75, 75));
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, dimensions);

            EventManager.QueueEvent(evt);
        }

        private void createAndInitializeAliens()
        {
            createRowOfAliens("alien_ray", Height - 80, 15, 48);
            createRowOfAliens("alien_pincher", Height - 160, 12, 60);
            createRowOfAliens("alien_pincher", Height - 240, 12, 60);
            createRowOfAliens("alien_hammerhead", Height - 325, 8, 90);
            createRowOfAliens("alien_hammerhead", Height - 410, 8, 90);
        }

        private void createRowOfAliens(string alienType, int posY, int number, int margin)
        {
            int posX = 160;
            for (int i = 0; i < number; ++i) {
                createAndInitializeAlien(alienType, posX, posY);
                posX += margin;
            }
        }

        private void createAndInitializeAlien(string type, float x, float y)
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, type);

            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(x, y));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);

            EventManager.QueueEvent(evt);
        }
        
        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case CreateEntityEvent.CREATE_ENTITY:
                    CreateEntityEvent createEvent = evt as CreateEntityEvent;
                    addEntity(createEvent.EntityType, createEvent.Attributes);
                    break;
                case DestroyEntityEvent.DESTROY_ENTITY:
                    DestroyEntityEvent destroyEvent = evt as DestroyEntityEvent;
                    removeEntity(destroyEvent.EntityID);
                    break;
            }
        }

        private void addEntity(string id, Dictionary<string, object> attributes = null)
        {
            Entity entity = EntityFactory.New(id, attributes);
            entitiesToAdd.Add(entity);
        }

        private void removeEntity(int entityID)
        {
            entitiesToRemove.Add(Entities[entityID]);
        }

        private void removeEntity(Entity entity)
        {
            entitiesToRemove.Add(entity);
        }

    }
}
