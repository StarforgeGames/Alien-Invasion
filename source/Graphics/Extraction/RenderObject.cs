using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using SlimDX;

namespace Graphics
{
    public class RenderObject
    {
        public readonly ResourceHandle material, mesh;
        public readonly Matrix model;
        public readonly float frame;

        public RenderObject(ResourceHandle material, ResourceHandle mesh, Matrix model, float frame)
        {
            this.material = material;
            this.mesh = mesh;
            this.model = model;
            this.frame = frame;
        }
    }
}
