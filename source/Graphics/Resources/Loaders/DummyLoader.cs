using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources.Loaders
{
    public class TextResource : AResource
    {
        public string text;

        public override string ToString()
        {
            return text;
        }

    }

    public class DummyLoader : ABasicLoader
    {
        TextResource def = new TextResource { text = "default" };
        Random rand = new Random();

        protected override AResource DoLoad(string name)
        {
            TextResource res = new TextResource();
            res.text = name;
            System.Threading.Thread.Sleep(rand.Next(200));
            return res;
        }

        protected override void DoUnload(AResource resource)
        {
            TextResource res = resource as TextResource;
            res.text = "";
            System.Threading.Thread.Sleep(rand.Next(200));
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
