using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Resources;
using SlimDX.Direct3D10;
using SlimDX;

namespace Graphics.Resources
{
    public class MeshResource : AResource
    {
        public SlimDX.Direct3D10.Buffer buffer;
        public int elementSize;
        public int elementCount;

        internal DataStream stream;

        public PrimitiveTopology primitiveTopology;
        

        public SlimDX.Direct3D10.InputElement[] inputLayout { get; set; }
    }
}
