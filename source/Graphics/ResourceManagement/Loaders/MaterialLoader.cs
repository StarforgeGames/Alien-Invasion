using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;
using System.IO;

namespace Graphics.ResourceManagement.Loaders
{
    public class MaterialLoader : ABasicLoader
    {
        ResourceManager manager;

        string baseDirectory = @"Materials\";
        string extension = @".material";

        public MaterialLoader(ResourceManager manager)
        {
            this.manager = manager;
        }

        protected override Resources.AResource doLoad(string name)
        {
            // not implemented yet:
            // StringReader reader = new StringReader(File.ReadAllText(baseDirectory + name + extension));

            // currently dummy code since true loading is not implemented yet.
            MaterialResource res = new MaterialResource(
                effectHandle: manager.GetResource("default", "effect"),
                textureHandle: manager.GetResource(name, "texture"));
            return res;
        }

        protected override void doUnload(Resources.AResource resource)
        {
            // do nothing since the material resource does not consume much memory and will be collected by the gc.
        }

        public override string Type
        {
            get { return "material"; }
        }
    }
}
