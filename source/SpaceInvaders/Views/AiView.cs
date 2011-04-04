using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.EventManagement.Events;
using Game.EventManagement;
using Game;

namespace SpaceInvaders.Views
{
    class AiView : IGameView
    {
        public GameLogic Game { get; private set; }
        public IEventManager EventManager { get; private set; }

        public GameViewType Type { get { return GameViewType.PlayerView; } }
        public int ID
        {
            get { throw new NotImplementedException(); }
        }

        private List<Entity> invaders;


        public AiView(GameLogic game)
        {
            this.Game = game;
            this.EventManager = game.EventManager;

            this.invaders = new List<Entity>();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
            EventManager.AddListener(this, typeof(GameStateChangedEvent));
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity invader in invaders) {

            }
        }

        public void OnAttach(Entity entity)
        {
            invaders.Add(entity);
        }

        public void OnDetach(Entity entity)
        {
            if (invaders.Contains(entity)) {
                invaders.Remove(entity);
            }
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY: {
                    NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
                    Entity entity = Game.Entities[newEntityEvent.EntityID];
                    if (entity.Type.StartsWith("alien_")) {
                        OnAttach(entity);
                    }
                    break;
                }
                case DestroyEntityEvent.DESTROY_ENTITY: {
                    DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
                    Entity entity = Game.Entities[destroyEntityEvent.EntityID];
                    if (entity.Type.StartsWith("alien_")) {
                        OnDetach(entity);
                    }
                    break;
                }
            }
        }

    }
}
