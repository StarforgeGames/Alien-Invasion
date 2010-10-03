using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Graphics.Resources
{
    public abstract class AResource : IDisposable
    {
        private int acuiredCount = 0;

        public bool IsAcquired
        {
            get
            {
                return acuiredCount > 0;
            }
        }
    
        public void Acquire()
        {
            Interlocked.Increment(ref acuiredCount);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Interlocked.Decrement(ref acuiredCount);
        }

        #endregion
    }
}
