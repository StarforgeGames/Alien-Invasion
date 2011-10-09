using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Collections
{
    static class IntegerExtensions
    {
        public static void timesRepeat(this uint x, Action action)
        {
            for (uint i = 0; i < x; ++i)
            {
                action();
            }
        }
    }
}
