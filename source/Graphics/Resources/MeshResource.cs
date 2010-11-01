using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Resources;
using SlimDX.Direct3D10;

namespace Graphics.Resources
{
    class MeshResource : AResource
    {
        public SlimDX.Direct3D10.Buffer buffer;
        public int size;

        public PrimitiveTopology primitiveTopology;

        public SlimDX.Direct3D10.InputElement[] inputLayout { get; set; }
    }
}
