using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceInvaders.Input;
using Game.Entities;
using Graphics;
using System.Windows.Forms;
using System.Drawing;
using Game.EventManagement.Events;
using Game;

namespace SpaceInvaders.Views
{

    class PlayerView : IGameView, IDisposable
    {
        public GameLogic Game { get; private set; }
        public Renderer Renderer { get; private set; }
        public Form RenderForm { get; private set; }

        public GameViewType Type { get { return GameViewType.PlayerView; } }
        public int ID
        {
            get { throw new NotImplementedException(); }
        }

        private Entity playerEntity;
        private PlayerController controller;

        public PlayerView(GameLogic game)
        {
            this.Game = game;

            RenderForm = new Form();
            RenderForm.Size = new Size(800, 600);
            RenderForm.Text = "Space Invaders";

            Renderer = new Graphics.Renderer(RenderForm);
            Renderer.Start();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            Game.EventManager.AddListener(this, typeof(NewEntityEvent));
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
                    if (entity.Name == "player") {
                        OnAttach(entity);
                    }
                    break;
            }
        }

        public void Dispose()
        {
            Renderer.Stop();
            Renderer.Dispose();
        }
    }

}
