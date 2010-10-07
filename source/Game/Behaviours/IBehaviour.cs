using System;
using System.Collections.ObjectModel;
using Game.EventManagement.Events;
using Game.EventManagement;

namespace Game.Behaviours
{

    public interface IBehaviour : IEventListener
    {
        ReadOnlyCollection<Type> SupportedMessages { get; }

        void OnUpdate(float deltaTime);
    }

}
