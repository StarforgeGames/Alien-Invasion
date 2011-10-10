using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Threading;

namespace Utility.Threading
{
    public class CommandQueue : IAsyncExecutor
    {
        private BlockingCollection<Action> queue = new BlockingCollection<Action>();
        private CancellationTokenSource cts = new CancellationTokenSource();

        public void Add(Action command)
        {
            queue.Add(command);
        }

        public void Execute()
        {
            try
            {
                Action command = queue.Take(cts.Token);
                command();
            }
            catch (Exception)
            {
                var oldCts = cts;
                cts = new CancellationTokenSource();
                oldCts.Dispose();
                // ignore
            }
        }

        public bool TryExecute()
        {
            Action command;
            if(queue.TryTake(out command))
            {
                command();
                return true;
            }
            return false;
        }

        public uint TryExecute(uint count)
        {
            for (uint i = 0; i < count; ++i )
            {
                if (!TryExecute())
                {
                    return i;
                }
            }
            return count;
        }

        public void Cancel()
        {
            cts.Cancel();
        }
    }
}
