﻿using System;
using System.Collections.Generic;
using Game;
using Game.EventManagement.Events;
using ResourceManagement;
using ResourceManagement.Debug;
using ResourceManagement.Loaders;
using ResourceManagement.Wipers;
using SlimDX.Windows;
using SpaceInvaders.Views;

namespace SpaceInvaders
{

    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application
    {
        public double GameLifeTime { get; private set; }

        public GameLogic Game { get; private set; }
        public List<IGameView> Views { get; private set; }

        private GameTimer timer = new GameTimer();
        private ResourceManager resourceManager = new ResourceManager(new ThreadPoolExecutor());
        
       

        public Application()
        {
            Game = new GameLogic(800, 600, resourceManager);

            Views = new List<IGameView>();
            PlayerView playerView = new PlayerView(Game);
            Views.Add(playerView);

            resourceManager.AddLoader(new DummyLoader());
            resourceManager.AddWiper(new DebugWiper());
            resourceManager.AddWiper(new FileUpdater());

            Game.EventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.Menu));
        }

        public void Update(float deltaTime)
        {
            GameLifeTime += deltaTime;
            Game.Update(deltaTime);

            foreach (IGameView view in Views) {
                view.OnUpdate(deltaTime);
            }
        }

        /// <summary>
        /// Run main loop.
        /// </summary>
        public void Run()
        {
            timer.Reset();
            PlayerView playerView = Views.Find(x => x.Type == GameViewType.PlayerView) as PlayerView;

            MessagePump.Run(playerView.RenderForm, () =>
            {
                timer.Tick();
                float deltaTime = timer.DeltaTime;
                Update(deltaTime);
            });

            playerView.Dispose();

            resourceManager.Dispose();
        }
    }

}
