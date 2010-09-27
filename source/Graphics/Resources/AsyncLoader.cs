using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    class AsyncLoader
    {
        IResourceLoader loader;

        AsyncLoader(IResourceLoader loader)
        {
            this.loader = loader;
        }

        public string[]  FileTypes
        {
            get { throw new NotImplementedException(); }
        }

        public void Load(string name, ref AResource resource)
        {
            throw new NotImplementedException();
        }

        public void Unload(ref AResource resource)
        {
            throw new NotImplementedException();
        }
    }
}
