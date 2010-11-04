using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Resources;

namespace ResourceManagement.Loaders
{
    public interface IResourceLoader : ILoader
    {
        string Type
        {
            get;
        }

        ResourceHandle Default
        {
            get;
        }
    }
}
