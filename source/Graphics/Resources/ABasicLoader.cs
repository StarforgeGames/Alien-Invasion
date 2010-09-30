using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Graphics.Resources
{
    public abstract class ABasicLoader : IResourceLoader
    {


        abstract protected AResource DoLoad(string name);

        abstract protected void DoUnload(AResource resource);

        #region IResourceLoader Members

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            Load(resourceHandle);
            evt.Finish();
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            Unload(resourceHandle);
            evt.Finish();
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            Reload(resourceHandle);
            evt.Finish();
        }

        public void Load(ResourceHandle resourceHandle)
        {
            try
            {
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Loading;
                resourceHandle.resources[resourceHandle.InactiveSlot].resource
                    = DoLoad(resourceHandle.Name);
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Ready;
                resourceHandle.Swap();
            }
            finally
            {
                resourceHandle.Finished();
            }
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            try
            {
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Unloading;
                DoUnload(resourceHandle.resources[resourceHandle.InactiveSlot].resource);
                resourceHandle.resources[resourceHandle.InactiveSlot].resource = null;
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Empty;
            }
            finally
            {
                resourceHandle.Finished();
            }
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            try
            {
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Unloading;
                DoUnload(resourceHandle.resources[resourceHandle.InactiveSlot].resource);
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Loading;
                resourceHandle.resources[resourceHandle.InactiveSlot].resource
                    = DoLoad(resourceHandle.Name);
                resourceHandle.resources[resourceHandle.InactiveSlot].status = ResourceState.Ready;
                resourceHandle.Swap();
            }
            finally
            {
                resourceHandle.Finished();
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
