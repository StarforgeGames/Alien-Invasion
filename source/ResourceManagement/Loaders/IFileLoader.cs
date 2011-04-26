using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceManagement.Loaders
{
    public interface IFileLoader
    {
        ResourceNameConverter Converter {get;}
    }
}
