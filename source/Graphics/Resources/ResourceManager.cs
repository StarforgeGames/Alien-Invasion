using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Graphics.Resources
{

    public interface IResourceLoader
    {

        string FileType
        {
            get;
        }

        AResource Default
        {
            get;
        }
        void Load(ResourceHandle resourceHandle);
        void Unload(ResourceHandle resourceHandle);

        void Reload(ResourceHandle resourceHandle);

        void Load(ResourceHandle resourceHandle, IEvent evt);
        void Unload(ResourceHandle resourceHandle, IEvent evt);

        void Reload(ResourceHandle resourceHandle, IEvent evt);
    }
    

    class ResourceManager
    {
        Dictionary<FileInfo, ResourceHandle> resources;
        List<AsyncLoader> Loaders { get; set; }
        public ResourceManager()
        {
            Loaders = new List<AsyncLoader>();
        }

        public ResourceHandle GetResource(FileInfo name)
        {
            throw new NotImplementedException();
            // suche loader
            return new ResourceHandle(name, null);
        }
    }
}
