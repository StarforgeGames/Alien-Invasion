using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Graphics.ResourceManagement.Resources
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

        abstract protected void Cleanup();
    
        public void Acquire()
        {
            Interlocked.Increment(ref acquiredCount);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Interlocked.Decrement(ref acquiredCount);
            if (acquiredCount == -1)
            {
                Cleanup();
                System.GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
