using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
