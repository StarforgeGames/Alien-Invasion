using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using System.Collections.ObjectModel;
using Game.EventManagement.Events;
using Game.EventManagement;

namespace Game.Behaviors
{

    abstract class AEntityBasedBehavior : IBehavior
    {
        protected Entity entity;

        public IEventManager EventManager { get; private set; }

        protected List<Type> handledEventTypes;
        public ReadOnlyCollection<Type> HandledEventTypes
        {
            get { return handledEventTypes.AsReadOnly(); }
        }

        public AEntityBasedBehavior(Entity entity)
        {
            this.entity = entity;
            this.EventManager = entity.EventManager;
        }

        public abstract void OnUpdate(float deltaTime);
        public abstract void OnEvent(Event evt);
    }

}
