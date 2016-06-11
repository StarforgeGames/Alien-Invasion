using System;
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
