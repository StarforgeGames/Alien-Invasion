using Game;
using Graphics;
using System.Windows.Forms;
using System.Drawing;
using System;
using SlimDX.Windows;

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

        Form form = new Form();

        public Application()
        {      
            form.Size = new Size(800, 600);
            form.Text = "Space Invaders";

            Button button = new Button();
            form.Controls.Add(button);

            renderer = new Graphics.Renderer(form);
            renderer.Start();

            Game = new BaseGame();
        }

        public void Update(float deltaTime)
        {
            int deltaMilliseconds = (int) deltaTime * 1000;
            Game.Update(deltaMilliseconds);
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
