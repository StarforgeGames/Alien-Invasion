using System;
using System.Collections.Generic;
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
        public Dictionary<string, IResourceLoader> AsyncLoaders { get; private set; }
        public Dictionary<string, IResourceLoader> Loaders { get; private set; }

        private Dictionary<string, Dictionary<string, ResourceHandle>> resourceHandles = 
            new Dictionary<string,Dictionary<string, ResourceHandle>>();
        private List<AWiper> wipers = new List<AWiper>();
    
        public ResourceManager(IAsyncExecutor executor)
        {
            AsyncExecutor = executor;
            AsyncLoaders = new Dictionary<string, IResourceLoader>();
            Loaders = new Dictionary<string, IResourceLoader>();
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
            lock (AsyncLoaders) {
                AsyncLoaders.Add(loader.Type, new AsyncLoader(loader, AsyncExecutor));
                lock (resourceHandles) {
                    resourceHandles.Add(loader.Type, new Dictionary<string, ResourceHandle>());
                }
                var dummy = AsyncLoaders[loader.Type].Default;
                lock (Loaders)
                {
                    Loaders.Add(loader.Type, loader);
                }
            }            
        }

        public void RemoveLoader(string type)
        {
            lock (AsyncLoaders) {
                doRemoveLoader(type);
            }
            lock (Loaders)
            {
                Loaders.Remove(type);
            }
        }

        private void doRemoveLoader(string type)
        {
            AsyncLoaders.Remove(type);
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
            
            lock (AsyncLoaders) {
                var types = AsyncLoaders.Keys.ToArray();
                foreach (var type in types) {
                    doRemoveLoader(type);
                }
                lock (Loaders)
                {
                    Loaders.Clear();
                }
	        }
        }
    }
}
