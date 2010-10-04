using Game;
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
        AWiper debugWiper = new Graphics.Resources.Debug.DebugWiper();

        public Application()
        {      
            form.Size = new Size(800, 600);
            form.Text = "Space Invaders";

            renderer = new Graphics.Renderer(form);
            renderer.Start();

            Game = new BaseGame(form.Size.Width, form.Size.Height);

            keyHandler = new PlayerController(Game.PlayerInterpreter);
            form.KeyDown += new KeyEventHandler(keyHandler.OnKeyDown);
            form.KeyUp += new KeyEventHandler(keyHandler.OnKeyUp);

            resourceManager = new ResourceManager(new ThreadPoolExecuter());
            
            resourceManager.AddLoader(new DummyLoader());
            resourceManager.AddWiper(debugWiper);
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
            debugWiper.Stop();
            renderer.Dispose();
        }
    }

}
