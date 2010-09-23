using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Resources
{

    public interface IResourceFile
    {
        bool Open(string file);
        long GetSize();
        int GetData(out byte[] buffer);
    }

}
