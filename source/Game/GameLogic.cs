using System.Collections.Generic;
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
        public GameWorld World { get; private set; }

		public IEventManager EventManager { get; private set; }
		public ProcessManager ProcessManager { get; private set; }
		public ResourceManager ResourceManager { get; private set; }
        public CollisionManager CollisionManager { get; private set; }

		public bool IsRunning { get { return State == GameState.Running; } }
		public bool IsPaused { get { return State != GameState.Running; } }

		public GameLogic(int worldWidth, int worldHeight, ResourceManager resourceManager)
		{
			State = GameState.StartUp;

			EventManager = new SwappingEventManager(this);
			ProcessManager = new ProcessManager();
			ResourceManager = resourceManager;
            CollisionManager = new CollisionManager(this);

            World = new GameWorld(this, worldWidth, worldHeight);

			registerGameEventListeners();
		}

		private void registerGameEventListeners()
		{
			EventManager.AddListener(this, typeof(GameStateChangedEvent));
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
                    initialize();

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
            World.Reset();
            CollisionManager.Reset();
			EventManager.Reset();
            ProcessManager.Reset();
		}

        private void initialize()
        {
            World.Initialize();
        }
		
		public void Update(float deltaTime)
        {
			EventManager.Tick();

            if (IsRunning) {
                World.Update(deltaTime);

                ProcessManager.Update(deltaTime);
                CollisionManager.DetectAndResolveCollisions();

                checkForVictoryConditions();
            }
		}

		private void checkForVictoryConditions()
		{
            bool playerIsDead = true;
            bool allAliensAreDead = true;
            foreach (Entity entity in World.Entities.Values) {
                if (allAliensAreDead && entity.Type.StartsWith("alien_")) {
                    allAliensAreDead = false;
				}
                else if (entity.Type == "player") {
                    playerIsDead = false;
                }
            }

            if (playerIsDead) {
                GameStateChangedEvent changeState = new GameStateChangedEvent(
                    GameStateChangedEvent.GAME_STATE_CHANGED, GameState.GameOver);
                EventManager.QueueEvent(changeState);
            }
            else if(allAliensAreDead) {
                GameStateChangedEvent changeState = new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                    GameState.Victory);
                EventManager.QueueEvent(changeState);
            }
		}		

		public void OnEvent(Event evt)
		{
			switch (evt.Type) {
				case GameStateChangedEvent.GAME_STATE_CHANGED:
					GameStateChangedEvent stateChangedEvent = evt as GameStateChangedEvent;
					changeState(stateChangedEvent.NewState);
					break;
			}
		}
	}

}
