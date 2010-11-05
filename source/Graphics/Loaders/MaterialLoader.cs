using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using System.IO;
using Graphics.Resources;

namespace Graphics.Loaders
{
    public class MaterialLoader : ABasicLoader, IFileLoader
    {
        ResourceManager manager;

        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\materials\", @".mtl");

        public MaterialLoader(ResourceManager manager)
        {
            this.manager = manager;
        }

        protected override AResource doLoad(string name)
        {
            // not implemented yet:
            // StringReader reader = new StringReader(File.ReadAllText(baseDirectory + name + extension));

            // currently dummy code since true loading is not implemented yet.
            MaterialResource res = new MaterialResource(
                effectHandle: manager.GetResource("default", "effect"),
                textureHandle: manager.GetResource(name, "texture"));
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            // do nothing since the material resource does not consume much memory and will be collected by the gc.
        }

        public override string Type
        {
            get { return "material"; }
        }
    }
}
