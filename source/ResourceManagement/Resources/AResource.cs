using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ResourceManagement.Resources
{
    public abstract class AResource : IDisposable
    {
        private int acquiredCount = 0;

        public bool IsAcquired
        {
            get
            {
                return acquiredCount > 0;
            }
        }

        public virtual void Acquire()
        {
            Interlocked.Increment(ref acquiredCount);
        }

        #region IDisposable Members

        public virtual void Dispose()
        {
            Interlocked.Decrement(ref acquiredCount);
        }

        #endregion
    }
}
