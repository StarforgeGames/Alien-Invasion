﻿using System.Collections.Generic;
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
            this.State = newState;

            switch (newState) {
                case GameState.Menu:
                    break;
                case GameState.Loading:
                    createAndInitializePlayer();
                    createAndInitializeAliens();

                    EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.InGame));
                    break;
                case GameState.InGame:
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        private void createAndInitializePlayer()
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "player");

            float startX = WorldWidth / 2f - (75f / 2f);
            float startY = WorldHeight - 100 - (75f / 2f);
            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);

            Rectangle rect = new Rectangle(position, 75, 75);
            Attribute<Rectangle> bounds = new Attribute<Rectangle>(rect);
            evt.AddAttribute(SpatialBehavior.Key_Bounds, bounds);

            EventManager.QueueEvent(evt);
        }

        private void createAndInitializeAliens()
        {
            CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "alien_ray");

            float startX = WorldWidth / 2f - (75f / 2f);
            float startY = 100;
            Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(startX, startY));
            evt.AddAttribute(SpatialBehavior.Key_Position, position);

            Rectangle rect = new Rectangle(position, 75, 75);
            Attribute<Rectangle> bounds = new Attribute<Rectangle>(rect);
            evt.AddAttribute(SpatialBehavior.Key_Bounds, bounds);

            EventManager.QueueEvent(evt);
        }

        public void Update(float deltaTime)
        {
            List<Entity> tmp = new List<Entity>(Entities.Values);
            foreach (Entity entity in tmp) {
                switch (entity.State) {
                    case EntityState.Active:
                        entity.Update(deltaTime);
                        break;
                    case EntityState.Inactive:
                        entity.Update(deltaTime);
                        break;
                }
            }

            EventManager.Tick();
            ProcessManager.OnUpdate(deltaTime);
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
                    Entities.Remove(destroyEvent.EntityID);

                    System.Console.WriteLine("[" + this.GetType().Name + "] Destroyed entity #" 
                        + destroyEvent.EntityID);
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
            Entities.Add(entity.ID, entity);
        }

        private void addEntity(Entity entity)
        {
            Entities.Add(entity.ID, entity);
        }
    }

}
