using System;
using System.Collections.ObjectModel;
using Game.EventManagement;

namespace Game.Behaviors
{

    public interface IBehavior : IEventListener
    {
        ReadOnlyCollection<Type> HandledEventTypes { get; }

        void OnUpdate(float deltaTime);
    }

}
