using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using ResourceManagement.Wipers;
using ResourceManagement.Loaders;

namespace ResourceManagement
{
    public class ResourceManager : IDisposable
    {
        public IAsyncExecutor AsyncExecutor { get; private set; }
        public ConcurrentDictionary<string, IResourceLoader> AsyncLoaders { get; private set; }
        public ConcurrentDictionary<string, IResourceLoader> Loaders { get; private set; }

        private Dictionary<string, Dictionary<string, ResourceHandle>> resourceHandles = 
            new Dictionary<string, Dictionary<string, ResourceHandle>>();
        private List<AWiper> wipers = new List<AWiper>();
    
        public ResourceManager(IAsyncExecutor executor)
        {
            AsyncExecutor = executor;
            AsyncLoaders = new ConcurrentDictionary<string, IResourceLoader>();
            Loaders = new ConcurrentDictionary<string, IResourceLoader>();
        }

        public ResourceHandle GetResource(string name, string type)
        {
            return GetResource(new ResourceIdentifier(name, type));
        }

        public ResourceHandle GetResource(ResourceIdentifier identifier)
        {
            try
            {
                if (identifier.Name == "default") {
                    return AsyncLoaders[identifier.Type].Default;
                }
                lock (resourceHandles) {
                    var resourcesOfSameType = resourceHandles[identifier.Type];

                    if (!resourcesOfSameType.ContainsKey(identifier.Name)) {
                        ResourceHandle handle = new ResourceHandle(identifier.Name, AsyncLoaders[identifier.Type]);
                        lock (resourcesOfSameType) {
                            resourcesOfSameType.Add(identifier.Name, handle);
                        }
                    }

                    return resourcesOfSameType[identifier.Name];
                }
            }
            catch (Exception e)
            {                
                throw new NotSupportedException("No loader available for type! \r\n\r\nMessage: " + e.Message);
            }
        }

        public void AddLoader(IResourceLoader loader)
        {
            var asyncLoader = new AsyncLoader(loader, AsyncExecutor);
            AsyncLoaders.AddOrUpdate(loader.Type, asyncLoader, (a, b) => asyncLoader);
            lock (resourceHandles) {
                resourceHandles.Add(loader.Type, new Dictionary<string, ResourceHandle>());
            }

            var dummy = AsyncLoaders[loader.Type].Default;
                
            Loaders.AddOrUpdate(loader.Type, loader, (a, b) => loader);
        }

        public void RemoveLoader(string type)
        {

            doRemoveLoader(type);
            IResourceLoader dummy;
            Loaders.TryRemove(type, out dummy);
        }

        private void doRemoveLoader(string type)
        {
            {
                IResourceLoader dummy;
                AsyncLoaders.TryRemove(type, out dummy);
            }
            if (resourceHandles.ContainsKey(type))
            {
                foreach (var handle in resourceHandles[type])
                {
                    IEvent evt1 = new BasicEvent();
                    handle.Value.Unload(evt1);

                    IEvent evt2 = new BasicEvent();
                    handle.Value.Unload(evt2);

                    evt1.Wait();
                    evt2.Wait();
                }
                resourceHandles[type] = null;
            }
        }

        public void AddWiper(AWiper wiper)
        {
            wiper.SetResources(resourceHandles);
            wiper.SetManager(this);
            wiper.Start();
            lock (wipers) {
                wipers.Add(wiper);
            }
        }

        public void RemoveWiper(AWiper wiper)
        {
            lock (wipers) {
                wipers.Remove(wiper);
            }
            wiper.Stop();
            wiper.SetResources(null);
            wiper.SetManager(null);
        }

        public void Dispose()
        {
            lock (wipers) {
                foreach (var wiper in wipers) {
                    wiper.Stop();
                    wiper.SetResources(null);
                    wiper.SetManager(null);
                }
                wipers.Clear();
            }

            foreach (var loader in AsyncLoaders)
            {
                doRemoveLoader(loader.Key);
            }
            Loaders.Clear();
        }
    }
}
