using System.Collections.Generic;
using Game;
using Game.EventManagement;
using Game.EventManagement.Debug;
using Game.EventManagement.Events;
using ResourceManagement;
using ResourceManagement.Debug;
using ResourceManagement.Loaders;
using ResourceManagement.Wipers;
using SlimDX.Windows;
using SpaceInvaders.Views;
using Utility;

namespace SpaceInvaders
{

    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application : IEventListener
    {
        public GameLogic Game { get; private set; }
        public List<IGameView> Views { get; private set; }

        private GameClock clock = new GameClock();
        private ResourceManager resourceManager = new ResourceManager(new ThreadPoolExecutor());

        public Application()
        {
            Game = new GameLogic(1024, 768, resourceManager);

            Views = new List<IGameView>();
            Views.Add(new PlayerView(Game));
            Views.Add(new AiView(Game));

            resourceManager.AddLoader(new DummyLoader());
            resourceManager.AddWiper(new DebugWiper());
            resourceManager.AddWiper(new FileUpdater());

            Game.EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.Menu));

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            Game.EventManager.AddListener(this, typeof(GameStateChangedEvent));
            Game.EventManager.AddListener(this, typeof(DebugEvent));
        }

        /// <summary>
        /// Run main loop.
        /// </summary>
        public void Run()
        {
            clock.Reset();

            using (PlayerView playerView = Views.Find(x => x.Type == GameViewType.PlayerView) as PlayerView) {

                MessagePump.Run(playerView.RenderForm, () => {
                    clock.Tick();
                    float deltaTime = clock.DeltaTime;
                    Update(deltaTime);
                });

            }

            resourceManager.Dispose();
        }

        public void Update(float deltaTime)
        {
            Game.Update(deltaTime);

            foreach (IGameView view in Views) {
                view.OnUpdate(deltaTime);
            }
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case GameStateChangedEvent.GAME_STATE_CHANGED: {
                    GameStateChangedEvent stateChangedEvent = evt as GameStateChangedEvent;

                    if (stateChangedEvent.NewState != GameState.Running) {
                        clock.Stop();
                    }
                    else {
                        clock.Start();
                    }
                    break;
                    }
                case DebugEvent.SINGLE_STEP: {
                        clock.SingleStep();
                        break;
                    }
                case DebugEvent.DECREASE_SPEED: {
                    clock.TimeScale -= 0.1f;
                    break;
                }
                case DebugEvent.INCREASE_SPEED: {
                    clock.TimeScale += 0.1f;
                    break;
                }
                case DebugEvent.RESET_SPEED: {
                    clock.TimeScale = 1.0f;
                    break;
                }
            }
        }
    }

}
