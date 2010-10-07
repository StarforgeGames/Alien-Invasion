using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement.Events;

namespace Game.EventManagement
{
    public interface IEventListener
    {
        void OnMessage(Event msg);
    }
}
