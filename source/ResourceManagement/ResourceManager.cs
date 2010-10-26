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
        Dictionary<string, Dictionary<string, ResourceHandle>> resourceHandles = new Dictionary<string,Dictionary<string, ResourceHandle>>();
        Dictionary<string, IResourceLoader> Loaders { get; set; }
        List<AWiper> wipers = new List<AWiper>();

        public IAsyncExecutor AsyncExecuter
        {
            get;
            private set;
        }
    
        public ResourceManager(IAsyncExecutor executer)
        {
            AsyncExecuter = executer;
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
                if (identifier.Name == "default")
                {
                    return Loaders[identifier.Type].Default;
                }
                lock (resourceHandles)
                {
                    var resourcesOfSameType = resourceHandles[identifier.Type];
                    if (resourcesOfSameType.ContainsKey(identifier.Name))
                    {
                        ResourceHandle handle = resourcesOfSameType[identifier.Name];
                        return handle;
                    }
                    else
                    {
                        ResourceHandle handle = new ResourceHandle(identifier.Name, Loaders[identifier.Type]);
                        lock (resourcesOfSameType)
                        {
                            resourcesOfSameType.Add(identifier.Name, handle);
                        }
                        return handle;
                    }
                }
            }
            catch (Exception e)
            {
                
                throw new NotSupportedException("No loader available for type! \r\n\r\nMessage: " + e.Message);
            }
        }

        public void AddLoader(IResourceLoader loader)
        {
            lock (Loaders)
            {
                Loaders.Add(loader.Type, new AsyncLoader(loader, AsyncExecuter));
                lock (resourceHandles)
                {
                    resourceHandles.Add(loader.Type, new Dictionary<string, ResourceHandle>());
                }
                var dummy = Loaders[loader.Type].Default;
            }
            
        }

        private void doRemoveLoader(string type)
        {
            Loaders.Remove(type);
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
            }
        }

        public void RemoveLoader(string type)
        {
            lock (Loaders)
            {
                doRemoveLoader(type);
            }
        }

        public void AddWiper(AWiper wiper)
        {
            wiper.SetResources(resourceHandles);
            wiper.SetManager(this);
            wiper.Start();
            lock (wipers)
            {
                wipers.Add(wiper);
            }
        }

        public void RemoveWiper(AWiper wiper)
        {
            lock (wipers)
            {
                wipers.Remove(wiper);
            }
            wiper.Stop();
            wiper.SetResources(null);
            wiper.SetManager(null);
        }

        #region IDisposable Members

        public void Dispose()
        {

            lock (wipers)
            {
                foreach (var wiper in wipers)
                {
                    wiper.Stop();
                    wiper.SetResources(null);
                    wiper.SetManager(null);
                }
                wipers.Clear();
            }
            
            lock (Loaders)
	        {
                var types = Loaders.Keys.ToArray();
                foreach (var type in types)
                {
                    doRemoveLoader(type);
                }
	        }
            
            
        }

        #endregion
    }
}
