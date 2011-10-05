using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagement
{
    public interface IAsyncExecutor
    {
        void Add(Action command);
    }
}
