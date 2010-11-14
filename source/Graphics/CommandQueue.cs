using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics
{
    public class CommandQueue
    {
        Queue<Action> queue = new Queue<Action>();
        
        public bool Empty
        {
            get
            {
                bool empty;
                lock (queue)
                {
                     empty = !queue.Any();
                }
                return empty;
            }
        }

        public void Execute()
        {
            Action command = null;
            lock (queue)
            {
                if (queue.Any())
                {
                    command = queue.Dequeue();
                }
            }
            if (command != null)
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
            lock (queue)
            {
                queue.Enqueue(action);
            }
        }

        public void ExecuteAll()
        {
            lock (queue)
            {
                foreach (var command in queue)
                {
                    command();
                }
                queue.Clear();
            }
        }

    }
}
