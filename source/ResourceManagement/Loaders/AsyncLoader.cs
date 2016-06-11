using Utility.Threading;

namespace ResourceManagement.Loaders
{
    class AsyncLoader : IResourceLoader
    {
        IResourceLoader loader;
        IAsyncExecutor executor;

        public AsyncLoader(IResourceLoader loader, IAsyncExecutor executor)
        {
            this.loader = loader;
            this.executor = executor;
        }


        #region IResourceLoader Members

        public string Type
        {
            get { return loader.Type; }
        }

        public ResourceHandle Default
        {
            get { return loader.Default; }
        }

        #endregion

        #region ILoader Members

        public void Load(ResourceHandle resourceHandle)
        {
            executor.Add(() =>
            {
                loader.Load(resourceHandle);
            });
        }

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Add(() =>
            {
                loader.Load(resourceHandle, evt);
            });
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            executor.Add(() =>
            {
                loader.Reload(resourceHandle);
            });
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Add(() =>
            {
                loader.Reload(resourceHandle, evt);
            });
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            executor.Add(() =>
            {
                loader.Unload(resourceHandle);
            });
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Add(() =>
            {
                loader.Unload(resourceHandle, evt);
            });
        }

        #endregion
    }
}
