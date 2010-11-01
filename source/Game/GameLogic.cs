using System.Collections.Generic;
using Game.Behaviors;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game.Utility;
using ResourceManagement;

namespace Game
{
    public enum GameState
    {
        StartUp,
        Menu,
        Loading,
        InGame,
        Paused,
        GameOver,
        Quit

    }

    public class GameLogic : IEventListener
    {
        public GameState State { get; private set; }
        public int WorldWidth { get; private set; }
        public int WorldHeight { get; private set; }

        public IEventManager EventManager { get; private set; }
        public ProcessManager ProcessManager { get; private set; }
        public ResourceManager ResourceManager { get; private set; }

        public EntityFactory EntityFactory { get; private set; }
        public Dictionary<int, Entity> Entities { get; private set; }

        public bool IsRunning { get { return State == GameState.InGame; } }
        public bool IsPaused { get { return State != GameState.InGame; } }

        private List<int> entitiesToRemove = new List<int>();
        private List<Entity> entitiesToAdd = new List<Entity>();

        public GameLogic(int worldWidth, int worldHeight, ResourceManager resourceManager)
        {
            State = GameState.StartUp;
            this.WorldWidth = worldWidth;
            this.WorldHeight = worldHeight;

            EventManager = new SwappingEventManager(this);
            ProcessManager = new ProcessManager();
            ResourceManager = resourceManager;

            EntityFactory = new EntityFactory(this);
            Entities = new Dictionary<int, Entity>();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(GameStateChangedEvent));
            EventManager.AddListener(this, typeof(CreateEntityEvent));
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
            //EventManager.AddListener(new EventLogger(), typeof(Event));
        }

        private void changeState(GameState newState)
        {
            GameState oldState = State;
            this.State = newState;

            switch (newState) {
                case GameState.Menu:
                    if (oldState == GameState.Menu) {
                        State = GameState.InGame;
                        EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                            GameState.InGame));
                    }
                    break;
                case GameState.Loading:
                    reset();
                    createAndInitializePlayer();
                    createAndInitializeAliens();

                    EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.InGame));
                    break;
                case GameState.InGame:
                    break;
                case GameState.Paused:
                    if (oldState == GameState.Paused) {
                        State = GameState.InGame;
                        EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                            GameState.InGame));
                    }
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        private void reset()
        {
            entitiesToAdd.Clear();
            entitiesToRemove.AddRange(Entities.Keys);

            EventManager.Reset();
            ProcessManager.Reset();
        }

        private void createAndInitializePlayer()
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "player");

            float startX = WorldWidth / 2f - (75f / 2f);
            float startY = WorldHeight - 50 - (75f / 2f);
            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);
            Attribute<Vector2D> dimensions = new Attribute<Vector2D>(new Vector2D(75, 75));
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, dimensions);

            EventManager.QueueEvent(evt);
        }

        private void createAndInitializeAliens()
        {
            createRowOfAliens("alien_ray", 60, 35, 35);
            createRowOfAliens("alien_ray", 105, 35, 35);
            createRowOfAliens("alien_pincher", 155, 50, 38);
            createRowOfAliens("alien_pincher", 200, 50, 38);
            createRowOfAliens("alien_hammerhead", 255, 75, 52);
            createRowOfAliens("alien_hammerhead", 325, 75, 52);
        }

        private void createRowOfAliens(string alienType, int posY, int width, int height)
        {
            int posX = 115;
            while (posX < WorldWidth - 100) {
                createAndInitializeAlien(alienType, posX, posY, width, height);
                posX += width + 25;
            }
        }

        private void createAndInitializeAlien(string type, float x, float y, float width, float height)
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, type);

            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(x, y));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);

            Attribute<Vector2D> dimensions = new Attribute<Vector2D>(new Vector2D(width, height));
            evt.AddAttribute(SpatialBehavior.Key_Dimensions, dimensions);

            EventManager.QueueEvent(evt);
        }

        public void Update(float deltaTime)
        {
            if (IsRunning) {
                simulate(deltaTime);
            }

            EventManager.Tick();

            addNewEntities();
            destroyEntities();
        }

        private void simulate(float deltaTime)
        {
            foreach (Entity entity in Entities.Values) {
                entity.Update(deltaTime);
            }

            ProcessManager.OnUpdate(deltaTime);
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
            foreach (int entityID in entitiesToRemove) {
                Entity entity = Entities[entityID];
                if (entity.Type == "player") {
                    GameStateChangedEvent changeState = new GameStateChangedEvent(
                        GameStateChangedEvent.GAME_STATE_CHANGED, GameState.GameOver);
                    EventManager.QueueEvent(changeState);
                }

                Entities.Remove(entityID);
                System.Console.WriteLine("[" + this.GetType().Name + "] Destroyed entity #" + entityID);
            }

            entitiesToRemove.Clear();
        }

        public void OnEvent(Event evt)
        {
            if (evt.RecipientID != 0) {
                return;
            }

            switch (evt.Type) {
                case CreateEntityEvent.CREATE_ENTITY:
                    CreateEntityEvent createEvent = evt as CreateEntityEvent;
                    addEntity(createEvent.EntityType, createEvent.Attributes);
                    break;
                case DestroyEntityEvent.DESTROY_ENTITY:
                    DestroyEntityEvent destroyEvent = evt as DestroyEntityEvent;
                    removeEntity(destroyEvent.EntityID);
                    break;
                case GameStateChangedEvent.GAME_STATE_CHANGED:
                    GameStateChangedEvent stateChangedEvent = evt as GameStateChangedEvent;
                    changeState(stateChangedEvent.NewState);
                    break;
            }
        }

        private void addEntity(string id, Dictionary<string, object> attributes = null)
        {
            Entity entity = EntityFactory.New(id, attributes);
            addEntity(entity);
        }

        private void addEntity(Entity entity)
        {
            entitiesToAdd.Add(entity);
        }

        private void removeEntity(int entityID)
        {
            entitiesToRemove.Add(entityID);
        }
    }

}
