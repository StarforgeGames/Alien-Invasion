using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
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

        public AResource Default
        {
            get { return loader.Default; }
        }

        #endregion

        #region ILoader Members

        public void Load(ResourceHandle resourceHandle)
        {
            executor.Execute(() =>
            {
                loader.Load(resourceHandle);
            });
        }

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Execute(() =>
            {
                loader.Load(resourceHandle, evt);
            });
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            executor.Execute(() =>
            {
                loader.Reload(resourceHandle);
            });
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Execute(() =>
            {
                loader.Reload(resourceHandle, evt);
            });
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            executor.Execute(() =>
            {
                loader.Unload(resourceHandle);
            });
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            executor.Execute(() =>
            {
                loader.Unload(resourceHandle, evt);
            });
        }

        #endregion
    }
}
