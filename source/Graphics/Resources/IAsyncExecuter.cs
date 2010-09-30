using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    public interface IAsyncExecuter
    {
        void Execute(Action command);
    }
}
