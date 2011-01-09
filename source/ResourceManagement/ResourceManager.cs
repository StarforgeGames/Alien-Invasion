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
        private const int ConcurrencyLevel = 4;

        public IAsyncExecutor AsyncExecutor { get; private set; }
        public ConcurrentDictionary<string, IResourceLoader> AsyncLoaders { get; private set; }
        public ConcurrentDictionary<string, IResourceLoader> Loaders { get; private set; }

        private ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>> resourceHandles =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>>(ConcurrencyLevel, 16);
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
                var resourcesOfSameType = resourceHandles[identifier.Type];
                    
                return resourcesOfSameType.GetOrAdd(
                    identifier.Name,
                    (name) => 
                        new ResourceHandle(identifier.Name, AsyncLoaders[identifier.Type]));
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

            if (resourceHandles.TryAdd(loader.Type, new ConcurrentDictionary<string, ResourceHandle>(16, 256)))
            {
                var dummy = AsyncLoaders[loader.Type].Default;

                Loaders.AddOrUpdate(loader.Type, loader, (a, b) => loader);
            }
            else
            {
                throw new NotSupportedException("Error: Loadertype '" + loader.Type + "' already registered.");
            }
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

            ConcurrentDictionary<string, ResourceHandle> handles;

            if (resourceHandles.TryGetValue(type, out handles))
            {
                foreach (var handle in handles)
                {
                    IEvent evt1 = new BasicEvent();
                    handle.Value.Unload(evt1);

                    IEvent evt2 = new BasicEvent();
                    handle.Value.Unload(evt2);

                    evt1.Wait();
                    evt2.Wait();
                }
                resourceHandles.TryRemove(type, out handles);
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
