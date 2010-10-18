using System;
using System.Collections.ObjectModel;
using Game.EventManagement.Events;
using Game.EventManagement;

namespace Game.Behaviors
{

    public interface IBehavior : IEventListener
    {
        ReadOnlyCollection<Type> HandledEventTypes { get; }
        IEventManager EventManager { get; }

        void OnUpdate(float deltaTime);
    }

}
