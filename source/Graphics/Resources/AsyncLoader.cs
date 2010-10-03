using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    class AsyncLoader : IResourceLoader
    {
        IResourceLoader loader;
        IAsyncExecuter executer;

        public AsyncLoader(IResourceLoader loader, IAsyncExecuter executer)
        {
            this.loader = loader;
            this.executer = executer;
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
            executer.Execute(() =>
            {
                loader.Load(resourceHandle);
            });
        }

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            executer.Execute(() =>
            {
                loader.Load(resourceHandle, evt);
            });
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            executer.Execute(() =>
            {
                loader.Reload(resourceHandle);
            });
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            executer.Execute(() =>
            {
                loader.Reload(resourceHandle, evt);
            });
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            executer.Execute(() =>
            {
                loader.Unload(resourceHandle);
            });
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            executer.Execute(() =>
            {
                loader.Unload(resourceHandle, evt);
            });
        }

        #endregion
    }
}
