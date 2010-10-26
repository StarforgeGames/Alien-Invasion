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
        public readonly Vector2 position, bounds;

        public RenderObject(ResourceHandle material, ResourceHandle mesh, Vector2 position, Vector2 bounds)
        {
            this.material = material;
            this.mesh = mesh;
            this.position = position;
            this.bounds = bounds;
        }
    }
}
