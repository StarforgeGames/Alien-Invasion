using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources.Loaders
{
    public class TextResource : AResource
    {
        public string text;
    }

    public class DummyLoader : ABasicLoader
    {
        TextResource def = new TextResource { text = "default" };

        protected override AResource DoLoad(string name)
        {
            TextResource res = new TextResource();
            res.text = name;
            return res;
        }

        protected override void DoUnload(AResource resource)
        {
            TextResource res = resource as TextResource;
            res.text = "";
        }

        public override string Type
        {
            get { return "txt"; }
        }

        public override AResource Default
        {
            get { return def; }
        }
    }
}
