using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace ResourceManagement.Wipers
{
    public abstract class AWiper
    {
        protected ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>> resources;
        protected ResourceManager manager;
        public void SetResources(ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>> resources)
        {
            this.resources = resources;
        }
        public void SetManager(ResourceManager manager)
        {
            this.manager = manager;
        }

        public abstract void Start();
        public abstract void Stop();
    }
}
