using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Graphics.Resources
{

    public interface IResourceLoader : ILoader
    {
        string Type
        {
            get;
        }

        AResource Default
        {
            get;
        }
    }

    public class ResourceManager
    {
        Dictionary<string, Dictionary<string, ResourceHandle>> resources = new Dictionary<string,Dictionary<string, ResourceHandle>>();
        Dictionary<string, IResourceLoader> Loaders { get; set; }

        public IAsyncExecuter AsyncExecuter
        {
            get;
            private set;
        }
    
        public ResourceManager(IAsyncExecuter executer)
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
                var resourcesOfSameType = resources[identifier.Type];
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
                lock (resources)
                {
                    resources.Add(loader.Type, new Dictionary<string, ResourceHandle>());
                }
            }
            
        }

        public void RemoveLoader(string type)
        {
            lock (Loaders)
            {
                Loaders.Remove(type);
            }
        }
    }
}
