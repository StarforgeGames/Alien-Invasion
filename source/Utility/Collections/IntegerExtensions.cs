using System;

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
