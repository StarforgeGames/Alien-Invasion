using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using System.Collections.Concurrent;
using System.Threading;

namespace Audio
{
    class BlockingCommandQueue : IAsyncExecutor
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

        public void Cancel()
        {
            cts.Cancel();
        }
    }
}
