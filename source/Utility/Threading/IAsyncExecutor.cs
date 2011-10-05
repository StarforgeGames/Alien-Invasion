using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Threading
{
    public interface IAsyncExecutor
    {
        void Add(Action command);
    }
}
