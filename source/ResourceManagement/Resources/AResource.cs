using System;
using System.Threading;

namespace ResourceManagement.Resources
{
    public abstract class AResource : IDisposable
    {
        private int acquiredCount = 0;
        public bool IsAcquired { get { return acquiredCount > 0; } }

        public virtual void Acquire()
        {
            Interlocked.Increment(ref acquiredCount);
        }

        public virtual void Dispose()
        {
            Interlocked.Decrement(ref acquiredCount);
        }
    }
}
