﻿using Game;
using Graphics;
using System.Windows.Forms;
using System.Drawing;
using System;
using SlimDX.Windows;
using Graphics.Resources;
using SpaceInvaders.Input;
using Graphics.Resources.Loaders;

namespace SpaceInvaders
{

    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application
    {
        public BaseGame Game { get; set; }
        private Renderer renderer;

        private GameTimer timer = new GameTimer();
        private ResourceManager resourceManager;

        Form form = new Form();
        IKeyboardHandler keyHandler;

        public Application()
        {      
            form.Size = new Size(800, 600);
            form.Text = "Space Invaders";

            //Button button = new Button();
            //form.Controls.Add(button);

            renderer = new Graphics.Renderer(form);
            renderer.Start();

            Game = new BaseGame();

            keyHandler = new PlayerController(Game.PlayerInterpreter);
            form.KeyDown += new KeyEventHandler(keyHandler.OnKeyDown);
            form.KeyUp += new KeyEventHandler(keyHandler.OnKeyUp);

            resourceManager = new ResourceManager(new ThreadPoolExecuter());
            resourceManager.AddWiper(new Graphics.Resources.Debug.DebugWiper());
            resourceManager.AddLoader(new DummyLoader());
            resourceManager.GetResource("blub1", "txt");
            resourceManager.GetResource("blub2", "txt");
            resourceManager.GetResource("blub3", "txt");
            resourceManager.GetResource("blub4", "txt");
            resourceManager.GetResource("blub5", "txt");
            resourceManager.GetResource("blub6", "txt");
        }

        public void Update(float deltaTime)
        {
            Game.Update(deltaTime);
        }

        /// <summary>
        /// Run main loop.
        /// </summary>
        public void Run()
        {
            timer.Reset();

            MessagePump.Run(form, () =>
            {
                timer.Tick();
                float deltaTime = timer.DeltaTime;
                Update(deltaTime);
            });

            renderer.Dispose();
        }
    }

}
