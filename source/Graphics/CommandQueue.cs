using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Graphics
{
    public class CommandQueue
    {
        ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();
        
        public bool Empty
        {
            get
            {
                return queue.IsEmpty;
            }
        }

        public void Execute()
        {
            Action command;
            if (queue.TryDequeue(out command))
            {
                command();
            }
        }

        public void Execute(uint maximum)
        {
            for (uint i = 0; i < maximum; ++i)
            {
                Execute();
            }
        }

        public void Add(Action action)
        {
            queue.Enqueue(action);
        }

        public void ExecuteAll()
        {
            var tempQueue = queue;
            queue = new ConcurrentQueue<Action>();

            foreach (var command in tempQueue)
            {
                command();
            }
        }

    }
}
