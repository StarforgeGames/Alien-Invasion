using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
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
