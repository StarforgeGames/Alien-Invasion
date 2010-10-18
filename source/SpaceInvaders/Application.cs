using System.Collections.Generic;
using Game;
using Game.EventManagement.Events;
using Graphics.ResourceManagement;
using Graphics.ResourceManagement.Debug;
using Graphics.ResourceManagement.Loaders;
using Graphics.ResourceManagement.Wipers;
using SlimDX.Windows;
using SpaceInvaders.Views;
using System;

namespace SpaceInvaders
{

    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application
    {
        public GameLogic Game { get; private set; }
        public List<IGameView> Views { get; private set; }

        public double LifeTime { get; private set; }

        private GameTimer timer = new GameTimer();
        private ResourceManager resourceManager;
        
        private List<IResourceLoader> rendererLoaders = new List<IResourceLoader>();

        AWiper debugWiper = new DebugWiper();

        public Application()
        {
            Game = new GameLogic(800, 600);

            Views = new List<IGameView>();
            PlayerView playerView = new PlayerView(Game);
            Views.Add(playerView);

            LifeTime = 0d;

            rendererLoaders.Add(new TextureLoader(playerView.Renderer));
            rendererLoaders.Add(new MeshLoader(playerView.Renderer));
            rendererLoaders.Add(new EffectLoader(playerView.Renderer));

            resourceManager = new ResourceManager(new ThreadPoolExecutor());
            resourceManager.AddLoader(new MaterialLoader(resourceManager));

            resourceManager.AddLoader(new DummyLoader());

            foreach (var rendererLoader in rendererLoaders)
            {
                resourceManager.AddLoader(rendererLoader);
            }

            

            resourceManager.AddWiper(debugWiper);

            // some testing code
            //resourceManager.GetResource("player", "texture");
            //using (var test1 = resourceManager.GetResource("player", "texture").Acquire())
            {
                using (var test2 = resourceManager.GetResource("quad", "mesh").Acquire())
                {
                    using (var test3 = resourceManager.GetResource("SimplePassThrough", "effect").Acquire())
                    {
                    }
                }
            }

            resourceManager.GetResource("default", "material");
            resourceManager.GetResource("player", "material");
            //System.Threading.Thread.Sleep(1000);
            //resourceManager.GetResource("quad", "mesh").Acquire();
            
            // end of testing code

            Game.ChangeState(GameState.Loading);
        }

        public void Update(float deltaTime)
        {
            LifeTime += deltaTime;
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

            resourceManager.Dispose();

            foreach (var rendererLoader in rendererLoaders)
            {
                if (rendererLoader is IDisposable)
                {
                    ((IDisposable)rendererLoader).Dispose();
                }
                
            }

            playerView.Dispose();
        }
    }

}
