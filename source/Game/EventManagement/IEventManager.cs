using System;
using Game.EventManagement.Events;

namespace Game.EventManagement
{

    public interface IEventManager
    {
        bool AddListener(IEventListener listener, Type eventType);
        bool RemoveListener(IEventListener listener, Type eventType);
        void Trigger(Event msg);
        bool Queue(Event msg);
        bool AbortEvent(Event msg);
        bool Tick();
        void Reset();
    }

}
