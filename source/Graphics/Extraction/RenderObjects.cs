using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement;

namespace Graphics
{
    class RenderObjects
    {
        private Dictionary<ResourceHandle, Dictionary<ResourceHandle, RenderObjects>> objs;

        internal void Clear()
        {
            objs.Clear();
        }
    }
}
