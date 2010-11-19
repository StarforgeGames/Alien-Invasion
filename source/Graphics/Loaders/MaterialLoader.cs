using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using System.IO;
using Graphics.Resources;
using LispInterpreter;

namespace Graphics.Loaders
{
    public class MaterialLoader : ABasicLoader, IFileLoader
    {
        ResourceManager manager;
        Interpreter inter = new Interpreter();

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
            //string text = File.ReadAllText(converter.getFilenameFrom(name));
            
            //inter.Eval(text.ToCharArray());

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
