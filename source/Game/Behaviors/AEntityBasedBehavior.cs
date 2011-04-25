using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace Game.Behaviors
{

    public abstract class AEntityBasedBehavior : IBehavior
    {
        protected Entity entity;
        protected GameLogic game;
        protected GameWorld world;
        protected IEventManager eventManager;

        protected List<Type> handledEventTypes = new List<Type>();
        public ReadOnlyCollection<Type> HandledEventTypes
        {
            get { return handledEventTypes.AsReadOnly(); }
        }

        public AEntityBasedBehavior(Entity entity)
        {
            this.entity = entity;
            this.game = entity.Game;
            this.world = game.World;
            this.eventManager = entity.EventManager;
        }

        protected virtual void initializeHandledEventTypes()
        { }

        public abstract void OnUpdate(float deltaTime);
        public abstract void OnEvent(Event evt);
    }

}
