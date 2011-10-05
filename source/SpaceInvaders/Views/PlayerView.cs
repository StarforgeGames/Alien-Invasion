using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Audio;
using Game;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;
using Graphics;
using Graphics.Loaders;
using Graphics.Loaders.Material;
using Graphics.Loaders.Mesh;
using ResourceManagement.Loaders;
using SpaceInvaders.Controls;
using SpaceInvaders.Input;
using Audio.Loaders;

namespace SpaceInvaders.Views
{

    class PlayerView : IGameView, IDisposable
    {
        public GameLogic Game { get; private set; }
        public IEventManager EventManager { get; private set; }

        public Renderer Renderer { get; private set; }
        private Extractor extractor;
        public Form RenderForm { get; private set; }

        private DefaultAudioPlayer audioPlayer;

        private GameMainMenu mainMenuControl;
        private PauseScreen pauseControl;
        private VictoryScreen victoryControl;
        private GameOverScreen gameOverControl;

        public GameViewType Type { get { return GameViewType.PlayerView; } }
        public int ID
        {
            get { throw new NotImplementedException(); }
        }

        private Entity playerEntity;

        private GameController gameController;
        private PlayerController playerController;

        private List<IResourceLoader> rendererLoaders = new List<IResourceLoader>();
        private SoundLoader soundLoader;

        public PlayerView(GameLogic game)
        {
            this.Game = game;
            this.EventManager = game.EventManager;

            /**
             * Initialize Graphics Subsystem 
             **/
            RenderForm = new Form();
            RenderForm.ClientSize = new Size(Game.World.Width, Game.World.Height);
            RenderForm.Text = "Alien Invasion v0.9";
            RenderForm.BackColor = Color.Empty;
            RenderForm.KeyPreview = true;

            gameController = new GameController(EventManager);
            RenderForm.KeyDown += new KeyEventHandler(gameController.OnKeyDown);
            RenderForm.KeyUp += new KeyEventHandler(gameController.OnKeyUp);
            

            extractor = new Extractor(game);
            Renderer = new Graphics.Renderer(RenderForm, extractor);
            Renderer.StartRender();
            
            rendererLoaders.Add(new TextureLoader(Renderer));
            rendererLoaders.Add(new MeshLoader(Renderer));
            rendererLoaders.Add(new EffectLoader(Renderer));
            
            foreach (var rendererLoader in rendererLoaders)
            {
                game.ResourceManager.AddLoader(rendererLoader);
            }
            

            game.ResourceManager.AddLoader(new MaterialLoader(game.ResourceManager));
            
            /**
            * Initialize Audio Subsystem 
            **/
            audioPlayer = new DefaultAudioPlayer();
            audioPlayer.Start();
            soundLoader = new SoundLoader(audioPlayer);
            game.ResourceManager.AddLoader(soundLoader);
            
            
            

            /**
            * Initialize GUI 
            **/
            mainMenuControl = new GameMainMenu(EventManager);
            mainMenuControl.Location = new Point(
                (RenderForm.ClientSize.Width - mainMenuControl.Width) / 2,
                (RenderForm.ClientSize.Height - mainMenuControl.Height) / 2);
            RenderForm.Controls.Add(mainMenuControl);

            pauseControl = new PauseScreen();
            pauseControl.Location = new Point(
                (RenderForm.ClientSize.Width - pauseControl.Width) / 2,
                (RenderForm.ClientSize.Height / 2) - pauseControl.Height);
            RenderForm.Controls.Add(pauseControl);

            victoryControl = new VictoryScreen(EventManager);
            victoryControl.Location = new Point(
                (RenderForm.ClientSize.Width - victoryControl.Width) / 2,
                (RenderForm.ClientSize.Height / 2) - victoryControl.Height);
            RenderForm.Controls.Add(victoryControl);

            gameOverControl = new GameOverScreen(EventManager);
            gameOverControl.Location = new Point(
                (RenderForm.ClientSize.Width - gameOverControl.Width) / 2,
                (RenderForm.ClientSize.Height / 2) - gameOverControl.Height);
            RenderForm.Controls.Add(gameOverControl);


            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(AudioEvent));
            EventManager.AddListener(this, typeof(NewEntityEvent));
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
            EventManager.AddListener(this, typeof(GameStateChangedEvent));
        }

        public void OnUpdate(float deltaTime)
        {
            extractor.OnUpdate(deltaTime);
        }

        public void OnAttach(Entity entity)
        {
            this.playerEntity = entity;

            playerController = new PlayerController(playerEntity);
            RenderForm.KeyDown += new KeyEventHandler(playerController.OnKeyDown);
            RenderForm.KeyUp += new KeyEventHandler(playerController.OnKeyUp);

            Console.WriteLine("[" + this.GetType().Name + "] New " + playerEntity + " found, attaching to controller");
        }

        public void OnDetach()
        {
            if (playerEntity == null) {
                return;
            }

            Console.WriteLine("[" + this.GetType().Name + "] Detaching " + playerEntity + " from controller");

            RenderForm.KeyUp -= new KeyEventHandler(playerController.OnKeyUp);
            RenderForm.KeyDown -= new KeyEventHandler(playerController.OnKeyDown);
            playerController = null;

            this.playerEntity = null;
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case AudioEvent.PLAY_SOUND: {
                    AudioEvent audioEvent = (AudioEvent)evt;
                    audioPlayer.PlaySound(audioEvent.SoundResource);
                    break;
                }
                case NewEntityEvent.NEW_ENTITY: {
                        NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                        Entity entity = Game.World.Entities[newEntityEvent.EntityID];
                        if (entity.Type == "player") {
                            OnAttach(entity);
                        }
                        break;
                    }
                case DestroyEntityEvent.DESTROY_ENTITY: {
                        DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
                        Entity entity = Game.World.Entities[destroyEntityEvent.EntityID];
                        if (entity.Type == "player") {
                            OnDetach();
                        }
                        break;
                    }
                case GameStateChangedEvent.GAME_STATE_CHANGED: {
                        GameStateChangedEvent stateChangedEvent = (GameStateChangedEvent)evt;
                        onGameStateChanged(stateChangedEvent.NewState);
                        break;
                    }
            }
        }

        private void onGameStateChanged(GameState newState)
        {
            switch (newState) {
                case GameState.StartUp:
                    mainMenuControl.Hide();
                    pauseControl.Hide();
                    victoryControl.Hide();
                    gameOverControl.Hide();
                    break;
                case GameState.Menu:
                    mainMenuControl.Show();
                    pauseControl.Hide();
                    victoryControl.Hide();
                    gameOverControl.Hide();
                    break;
                case GameState.Loading:
                    OnDetach();
                    mainMenuControl.Hide();
                    pauseControl.Hide();
                    victoryControl.Hide();
                    gameOverControl.Hide();;
                    break;
                case GameState.Running:
                    mainMenuControl.Hide();
                    pauseControl.Hide();
                    victoryControl.Hide();
                    gameOverControl.Hide();
                    break;
                case GameState.Paused:
                    pauseControl.Show();
                    victoryControl.Hide();
                    gameOverControl.Hide();
                    break;
                case GameState.Victory:
                    mainMenuControl.Hide();
                    pauseControl.Hide();
                    victoryControl.Show();
                    gameOverControl.Hide();
                    break;
                case GameState.GameOver:
                    mainMenuControl.Hide();
                    pauseControl.Hide();
                    victoryControl.Hide();
                    gameOverControl.Show();
                    break;
                case GameState.Quit:
                    RenderForm.Close();
                    break;
                default:
                    break;
            }

            RenderForm.Focus(); // Always focus RenderForm, else Key input won't be captured
        }

        public void Dispose()
        {
            Renderer.StopRender();
            Renderer.WaitForStateChange();

            foreach (var rendererLoader in rendererLoaders) {
                Game.ResourceManager.RemoveLoader(rendererLoader.Type);

                if (rendererLoader is IDisposable) {
                    ((IDisposable)rendererLoader).Dispose();
                }

            }

            Renderer.StopHandleCommands();
            Renderer.WaitForStateChange();
            Renderer.WaitForCompletion();
            
            Renderer.Dispose();

            Game.ResourceManager.RemoveLoader(soundLoader.Type);
            soundLoader.Dispose();

            audioPlayer.Stop();
            audioPlayer.Dispose();
        }
    }

}
