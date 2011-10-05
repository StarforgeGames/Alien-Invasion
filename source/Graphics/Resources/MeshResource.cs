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
        public SlimDX.Direct3D10.Buffer vertexBuffer;
        public SlimDX.Direct3D10.Buffer indexBuffer;

        public int elementSize;
        public int elementCount;

        public bool indexed;
        public int indexCount;

        internal DataStream vertexstream;
        internal DataStream indexstream;

        public SlimDX.DXGI.Format indexFormat;

        public PrimitiveTopology primitiveTopology;
        

        public SlimDX.Direct3D10.InputElement[] inputLayout { get; set; }
    }
}
