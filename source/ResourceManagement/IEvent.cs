using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagement
{
    public enum EventState
    {
        Finished, Failed, Pending
    }

    public interface IEvent
    {
        EventState Wait();
        EventState Wait(TimeSpan timeout);

        void Abort();
        void Finish();
    }
}
