using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace Game.EventManagement
{

    public interface IEventManager
    {
        bool AddListener(IEventListener listener, Type eventType);
        bool RemoveListener(IEventListener listener, Type eventType);
        void Trigger(Event msg);
        bool QueueEvent(Event msg);
        bool AbortEvent(Event msg);
        bool Tick();
    }

}
