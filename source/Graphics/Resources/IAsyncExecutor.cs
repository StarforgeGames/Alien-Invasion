using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    public interface IAsyncExecutor
    {
        void Execute(Action command);
    }
}
