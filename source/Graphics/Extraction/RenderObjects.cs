using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement;
using SlimDX;

namespace Graphics
{
    class RenderObjects
    {
        private Dictionary<ResourceHandle, Dictionary<ResourceHandle, RenderObjects>> objs = new Dictionary<ResourceHandle,Dictionary<ResourceHandle,RenderObjects>>();
        private Matrix camera;

        internal void Clear()
        {
            objs.Clear();
        }

        public void Add(RenderObject obj)
        {

        }

        internal void SetCamera(Matrix camera)
        {
            this.camera = camera;
        }
    }
}
