using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
{
    public class MaterialLoader : ABasicLoader
    {
        ResourceManager manager;

        string baseDirectory = @"Materials\";
        string extension = @".mat";

        public MaterialLoader(ResourceManager manager)
        {
            this.manager = manager;
        }

        protected override Resources.AResource doLoad(string name)
        {
            MaterialResource res = new MaterialResource();

            throw new NotImplementedException();
        }

        protected override void doUnload(Resources.AResource resource)
        {
            throw new NotImplementedException();
        }

        public override string Type
        {
            get { return "material"; }
        }
    }
}
