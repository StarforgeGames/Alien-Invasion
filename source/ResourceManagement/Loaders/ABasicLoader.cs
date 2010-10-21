using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ResourceManagement.Resources;

namespace ResourceManagement.Loaders
{
    public abstract class ABasicLoader : IResourceLoader
    {
        ResourceHandle defaultResource;

        abstract protected AResource doLoad(string name);

        abstract protected void doUnload(AResource resource);

        #region IResourceLoader Members

        public void Load(ResourceHandle handle, IEvent evt)
        {
            try
            {
                Load(handle);
                evt.Finish();
            }
            catch(Exception)
            {
                evt.Abort();
            }
        }

        public void Unload(ResourceHandle handle, IEvent evt)
        {
            try
            {
                Unload(handle);
                evt.Finish();
            }
            catch (Exception)
            {
                evt.Abort();
            }
        }

        public void Reload(ResourceHandle handle, IEvent evt)
        {
            try
            {
                Reload(handle);
                evt.Finish();
            }
            catch (Exception)
            {
                evt.Abort();
            }
        }

        public void Load(ResourceHandle handle)
        {
            try
            {
                handle.inactive.resource
                    = doLoad(handle.Name);

                handle.inactive.state = ResourceState.Ready;
                handle.Swap();
                
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

        abstract public string Type
        {
            get;
        }

        private void loadDefault()
        {
            defaultResource = new ResourceHandle("default", this);
            IEvent evt = new BasicEvent();
            defaultResource.Load(evt);
            evt.Wait();
        }

        public ResourceHandle Default
        {
            get
            {
                if (defaultResource == null)
                {
                    loadDefault();
                }
                return defaultResource;
            }
        }

        #endregion

        ~ABasicLoader()
        {
            if (defaultResource != null)
            {
                IEvent evt = new BasicEvent();
                defaultResource.Unload(evt);
                evt.Wait();
            }
        }
    }
}
