﻿using System.Collections.Generic;
using Game.Behaviors;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Debug;
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
		Running,
		Paused,
        GameOver,
        Victory,
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
        public CollisionDetector CollisionDetector { get; private set; }

		public EntityFactory EntityFactory { get; private set; }
		public Dictionary<int, Entity> Entities { get; private set; }

		public bool IsRunning { get { return State == GameState.Running; } }
		public bool IsPaused { get { return State != GameState.Running; } }

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
            CollisionDetector = new CollisionDetector(this);

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
						State = GameState.Running;
						EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
							GameState.Running));
					}
					break;
				case GameState.Loading:
					reset();
					createAndInitializePlayer();
					createAndInitializeAliens();

					EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
						GameState.Running));
					break;
				case GameState.Running:
					break;
				case GameState.Paused:
					if (oldState == GameState.Paused) {
						State = GameState.Running;
						EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
							GameState.Running));
					}
					break;
				case GameState.GameOver:
					break;
			}
		}

		private void reset()
		{
			entitiesToAdd.Clear();
            entitiesToRemove.Clear();

            Entities.Clear();

			EventManager.Reset();
			ProcessManager.Reset();
		}

		private void createAndInitializePlayer()
		{
			CreateEntityEvent evt = new CreateEntityEvent(CreateEntityEvent.CREATE_ENTITY, "player");

			float startX = WorldWidth / 2f - (75f / 2f);
			float startY = 50;
			Attribute<Vector2D> position = new Attribute<Vector2D>(new Vector2D(startX, startY));
			evt.AddAttribute(SpatialBehavior.Key_Position, position);
			Attribute<Vector2D> dimensions = new Attribute<Vector2D>(new Vector2D(75, 75));
			evt.AddAttribute(SpatialBehavior.Key_Dimensions, dimensions);

			EventManager.QueueEvent(evt);
		}

		private void createAndInitializeAliens()
		{
			createRowOfAliens("alien_ray", WorldHeight - 80, 15, 48);
			createRowOfAliens("alien_pincher", WorldHeight - 160, 12, 60);
			createRowOfAliens("alien_pincher", WorldHeight - 240, 12, 60);
			createRowOfAliens("alien_hammerhead", WorldHeight - 325, 8, 90);
			createRowOfAliens("alien_hammerhead", WorldHeight - 410, 8, 90);
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

		public void Update(float deltaTime)
        {
			EventManager.Tick();

            if (IsRunning) {
                addNewEntities();
                destroyEntities();

                simulate(deltaTime);
                checkForVictoryConditions();
            }
		}

		private void simulate(float deltaTime)
		{
			foreach (Entity entity in Entities.Values) {
				entity.Update(deltaTime);
			}

			ProcessManager.OnUpdate(deltaTime);
            CollisionDetector.CheckForCollisionsInWorld();
		}

		private void checkForVictoryConditions()
		{
            foreach (Entity entity in Entities.Values) {
				if (entity.Type.StartsWith("alien_")) {
					return;
				}
			}

			GameStateChangedEvent changeState = new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
				GameState.Victory);
			EventManager.QueueEvent(changeState);
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
				if (this.IsRunning && entity.Type == "player") {
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
