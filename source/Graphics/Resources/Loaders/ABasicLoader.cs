using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Graphics.Resources.Loaders
{
    public abstract class ABasicLoader : IResourceLoader
    {


        abstract protected AResource doLoad(string name);

        abstract protected void doUnload(AResource resource);

        #region IResourceLoader Members

        public void Load(ResourceHandle handle, IEvent evt)
        {
            Load(handle);
            evt.Finish();
        }

        public void Unload(ResourceHandle handle, IEvent evt)
        {
            Unload(handle);
            evt.Finish();
        }

        public void Reload(ResourceHandle handle, IEvent evt)
        {
            Reload(handle);
            evt.Finish();
        }

        public void Load(ResourceHandle handle)
        {
            try
            {
                handle.inactive.resource
                    = doLoad(handle.Name);
                handle.Swap();
                handle.active.state = ResourceState.Ready;
                
            }
            finally
            {
                handle.Finished();
            }
        }

        public void Unload(ResourceHandle handle)
        {
            try
            {
                doUnload(handle.inactive.resource);
                handle.inactive.resource = null;
                handle.inactive.state = ResourceState.Empty;
            }
            finally
            {
                handle.Finished();
            }
        }

        public void Reload(ResourceHandle handle)
        {
            try
            {
                doUnload(handle.inactive.resource);
                handle.inactive.state = ResourceState.Loading;
                handle.inactive.resource
                    = doLoad(handle.Name);
                
                handle.Swap();
                handle.active.state = ResourceState.Ready;
            }
            finally
            {
                handle.Finished();
            }
        }

        #endregion

        #region IResourceLoader Members

        abstract public string Type
        {
            get;
        }

        abstract public AResource Default
        {
            get;
        }

        #endregion
    }
}
