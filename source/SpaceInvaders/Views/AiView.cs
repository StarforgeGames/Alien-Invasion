using System;
using System.Collections.Generic;
using Game;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game.Utility;
using Game.Behaviors;

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
        private Vector2D currentDirection;

        private float moveDownTime = 0.0f;
        private const float totalMoveDownTime = 0.5f;

        private bool movementDirectionChanged = false;

        private float timeSinceLastShot = 0.0f;
        private Random rng = new Random();


        public AiView(GameLogic game)
        {
            this.Game = game;
            this.EventManager = game.EventManager;

            this.invaders = new List<Entity>();

            registerGameEventListeners();
        }

        private void registerGameEventListeners()
        {
            EventManager.AddListener(this, typeof(NewEntityEvent));
            EventManager.AddListener(this, typeof(DestroyEntityEvent));
            EventManager.AddListener(this, typeof(GameStateChangedEvent));
            EventManager.AddListener(this, typeof(AiUpdateMovementEvent));
        }

        public void OnUpdate(float deltaTime)
        {
            if (Game.IsPaused || invaders.Count < 1) {
                return;
            }

            // TODO: Shoot!
            timeSinceLastShot += deltaTime;
            Entity shooter = invaders[rng.Next(invaders.Count - 1)];
            Attribute<float> firingSpeed = (Attribute<float>)shooter[CombatBehavior.Key_FiringSpeed];

            if (timeSinceLastShot * rng.NextDouble() * firingSpeed >= 1.0f) {
                timeSinceLastShot = 0.0f;                
                EventManager.Trigger(new FireWeaponEvent(FireWeaponEvent.FIRE_SINGLE_SHOT, shooter.ID));
            }

            if (currentDirection.Y < 0) {
                if (moveDownTime >= totalMoveDownTime) {
                    currentDirection.Y = 0.0f;
                    moveDownTime = 0.0f;
                    movementDirectionChanged = true;
                }

                moveDownTime += deltaTime;
            }

            if (!movementDirectionChanged) {
                return;
            }

            foreach (Entity invader in invaders) {
                MoveEvent move = new MoveEvent(MoveEvent.START_MOVING, invader.ID, currentDirection);
                EventManager.QueueEvent(move);
            }

            movementDirectionChanged = false;
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case NewEntityEvent.NEW_ENTITY: {
                    NewEntityEvent newEntityEvent = evt as NewEntityEvent;
                    Entity entity = Game.Entities[newEntityEvent.EntityID];
                    if (entity.Type.StartsWith("alien_")) {
                        OnAttach(entity);
                    }
                    break;
                }
                case DestroyEntityEvent.DESTROY_ENTITY: {
                    DestroyEntityEvent destroyEntityEvent = evt as DestroyEntityEvent;
                    Entity entity = Game.Entities[destroyEntityEvent.EntityID];
                    if (entity.Type.StartsWith("alien_")) {
                        OnDetach(entity);
                    }
                    break;
                }
                case GameStateChangedEvent.GAME_STATE_CHANGED: {
                    GameStateChangedEvent stateChangedEvent = evt as GameStateChangedEvent;
                    onGameStateChanged(stateChangedEvent.NewState);
                    break;
                    }
                case AiUpdateMovementEvent.AT_BORDER: {
                    AiUpdateMovementEvent aiMovementUpdateEvent = evt as AiUpdateMovementEvent;

                    Vector2D borderData = aiMovementUpdateEvent.BorderData;

                    if (borderData.Y < 0) {
                        // Victory for the Invaders!
                        EventManager.Trigger(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                            GameState.GameOver));
                    }

                    currentDirection.X = -borderData.X;
                    currentDirection.Y = -1;

                    movementDirectionChanged = true;
                    break;
                }
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

        private void onGameStateChanged(GameState newState)
        {
            switch (newState) {               
                case GameState.Loading:
                    currentDirection = new Vector2D(1, 0);
                    movementDirectionChanged = true;
                    invaders.Clear();
                    break;              
                default:
                    break;
            }
        }

    }
}
