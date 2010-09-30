using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    public class ThreadPoolExecuter : IAsyncExecuter
    {
        #region IAsyncExecuter Members

        public void Execute(Action command)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((object a) =>
            {
                command();
            });
        }

        #endregion
    }
}
