using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
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
        Random rand = new Random();

        protected override AResource doLoad(string name)
        {
            TextResource res = new TextResource();
            res.text = name;
            System.Threading.Thread.Sleep(rand.Next(1000));
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            TextResource res = (TextResource)resource;
            res.text = "";
            System.Threading.Thread.Sleep(rand.Next(1000));
        }

        public override string Type
        {
            get { return "txt"; }
        }
    }
}
