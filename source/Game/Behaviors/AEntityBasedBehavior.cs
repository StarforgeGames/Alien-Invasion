using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace Game.Behaviors
{

    abstract class AEntityBasedBehavior : IBehavior
    {
        protected Entity entity;

        public IEventManager EventManager { get; private set; }

        protected List<Type> handledEventTypes = new List<Type>();
        public ReadOnlyCollection<Type> HandledEventTypes
        {
            get { return handledEventTypes.AsReadOnly(); }
        }

        public AEntityBasedBehavior(Entity entity)
        {
            this.entity = entity;
            this.EventManager = entity.EventManager;

            initializeHandledEventTypes();
        }

        protected virtual void initializeHandledEventTypes()
        { }

        public abstract void OnUpdate(float deltaTime);
        public abstract void OnEvent(Event evt);
    }

}
