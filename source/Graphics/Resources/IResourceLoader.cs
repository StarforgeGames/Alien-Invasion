using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources
{
    public interface IResourceLoader : ILoader
    {
        string Type
        {
            get;
        }

        AResource Default
        {
            get;
        }
    }
}
