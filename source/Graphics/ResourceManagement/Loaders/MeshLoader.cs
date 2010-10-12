using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.ResourceManagement.Loaders
{
    class MeshLoader : IResourceLoader
    {
        Renderer renderer;

        MeshLoader(Renderer renderer)
        {
            this.renderer = renderer;
        }

        #region IResourceLoader Members

        public string Type
        {
            get { return "mesh"; }
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
