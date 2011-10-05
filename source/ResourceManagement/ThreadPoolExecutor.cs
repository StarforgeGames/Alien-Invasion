using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utility.Threading;

namespace ResourceManagement
{
    public class ThreadPoolExecutor : IAsyncExecutor
    {
        #region IAsyncExecutor Members

        public void Add(Action command)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object a) =>
            {
                command();
            });
        }

        #endregion
    }
}
