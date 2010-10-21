using System;
using System.Drawing;
using System.Windows.Forms;
using Game;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;
using Graphics;
using Graphics.Loaders;
using SpaceInvaders.Input;
using SpaceInvaders.Menus;
using ResourceManagement;
using ResourceManagement.Loaders;
using System.Collections.Generic;

namespace SpaceInvaders.Views
{

    class PlayerView : IGameView, IDisposable
    {
        public GameLogic Game { get; private set; }
        public IEventManager EventManager { get; private set; }
        public Renderer Renderer { get; private set; }
        private Extractor extractor;
        public Form RenderForm { get; private set; }

        private GameMainMenu mainMenuControl;

        public GameViewType Type { get { return GameViewType.PlayerView; } }
        public int ID
        {
            get { throw new NotImplementedException(); }
        }

        private Entity playerEntity;
        private PlayerController controller;

        private List<IResourceLoader> rendererLoaders = new List<IResourceLoader>();

        public PlayerView(GameLogic game, ResourceManager resourceManager)
        {
            this.Game = game;
            this.EventManager = game.EventManager;

            RenderForm = new Form();
            RenderForm.Size = new Size(800, 600);
            RenderForm.Text = "Space Invaders";
            RenderForm.BackColor = Color.Empty;

            extractor = new Extractor(game);
            Renderer = new Graphics.Renderer(RenderForm, extractor);

            rendererLoaders.Add(new TextureLoader(Renderer));
            rendererLoaders.Add(new MeshLoader(Renderer));
            rendererLoaders.Add(new EffectLoader(Renderer));

            foreach (var rendererLoader in rendererLoaders)
            {
                resourceManager.AddLoader(rendererLoader);
            }

            resourceManager.AddLoader(new MaterialLoader(resourceManager));



            mainMenuControl = new GameMainMenu(EventManager);
            mainMenuControl.Location = new Point((RenderForm.Width - mainMenuControl.Width) / 2, 100); ;
            RenderForm.Controls.Add(mainMenuControl);

            Renderer.Start();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(NewEntityEvent));
            EventManager.AddListener(this, typeof(GameStateChangedEvent));
        }

        public void OnUpdate(float deltaTime)
        {
            // TODO: Run Extractor and stuff
        }

        public void OnAttach(Entity entity)
        {
            this.playerEntity = entity;

            controller = new PlayerController(playerEntity);
            RenderForm.KeyDown += new KeyEventHandler(controller.OnKeyDown);
            RenderForm.KeyUp += new KeyEventHandler(controller.OnKeyUp);

            Console.WriteLine("[" + this.GetType().Name + "] New " + playerEntity + " found, attaching to controller");
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY:
                    NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                    Entity entity = Game.Entities[newEntityEvent.EntityID];
                    if (entity.Type == "player") {
                        OnAttach(entity);
                    }
                    break;
                case GameStateChangedEvent.GAME_STATE_CHANGED:
                    GameStateChangedEvent stateChangedEvent = (GameStateChangedEvent)evt;
                    onGameStateChanged(stateChangedEvent.NewState);
                    break;
            }
        }

        private void onGameStateChanged(GameState newState)
        {
            switch (newState) {
                case GameState.StartUp:
                    hideControl(mainMenuControl);
                    break;
                case GameState.Menu:
                    showControl(mainMenuControl);
                    break;
                case GameState.Loading:
                    hideControl(mainMenuControl);
                    break;
                case GameState.InGame:
                    break;
                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
                case GameState.Quit:
                    RenderForm.Close();
                    break;
                default:
                    break;
            }
        }

        private void showControl(Control control)
        {
            control.Enabled = true;
            control.Visible = true;
            control.Focus();
        }

        private void hideControl(Control control)
        {
            control.Enabled = false;
            control.Visible = false;
            RenderForm.Focus();
        }

        public void Dispose()
        {
            foreach (var rendererLoader in rendererLoaders)
            {
                if (rendererLoader is IDisposable)
                {
                    ((IDisposable)rendererLoader).Dispose();
                }

            }

            Renderer.Stop();
            Renderer.Dispose();
        }
    }

}
