using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Graphics.ResourceManagement
{
    public class BasicEvent : IEvent
    {
        EventWaitHandle handle = new EventWaitHandle(false, EventResetMode.AutoReset);
        EventState state = EventState.Pending;

        #region IEvent Members

        public EventState Wait()
        {
            state = EventState.Pending;
            handle.WaitOne();
            return state;
        }

        public EventState Wait(TimeSpan timeout)
        {
            state = EventState.Pending;
            handle.WaitOne(timeout);
            return state;
        }

        public void Abort()
        {
            state = EventState.Failed;
            handle.Set();
        }

        public void Finish()
        {
            state = EventState.Finished;
            handle.Set();
        }

        #endregion
    }
}
