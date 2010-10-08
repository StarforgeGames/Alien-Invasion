using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
{
    abstract class ARendererLoader : IResourceLoader
    {
        protected Renderer renderer;

        abstract protected AResource doLoad(string name);

        abstract protected void doUnload(AResource resource);

        public ARendererLoader(Renderer renderer)
        {
            this.renderer = renderer;
        }

        #region IResourceLoader Members

        public string Type
        {
            get { throw new NotImplementedException(); }
        }

        public Resources.AResource Default
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ILoader Members

        public void Load(ResourceHandle resourceHandle)
        {
            throw new NotImplementedException();
        }

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            throw new NotImplementedException();
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            throw new NotImplementedException();
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            throw new NotImplementedException();
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            throw new NotImplementedException();
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
