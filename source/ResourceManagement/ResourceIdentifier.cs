using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagement
{
    public class ResourceIdentifier
    {
        public string Name
        {
            get;
            private set;
        }
        public string Type
        {
            get;
            private set;
        }

        public ResourceIdentifier(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
