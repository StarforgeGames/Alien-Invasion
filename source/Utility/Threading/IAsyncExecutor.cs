using System;

namespace Utility.Threading
{
    public interface IAsyncExecutor
    {
        void Add(Action command);
    }
}
