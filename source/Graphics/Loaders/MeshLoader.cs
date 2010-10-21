using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX;
using SlimDX.Direct3D10;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using Graphics.Resources;

namespace Graphics.Loaders
{
    public class MeshLoader : ARendererLoader<byte[]>
    {
        string baseDirectory = @"data\meshes\";
        string extension = @".mesh";

        public MeshLoader(Renderer renderer) : base(renderer)
        {
        }


        protected override byte[] ReadResourceWithName(string name)
        {
            return File.ReadAllBytes(baseDirectory + name + extension);
        }

        protected override AResource doLoad(byte[] data)
        {

            // currently dummy code since true loading is not implemented yet.
            MeshResource res = new MeshResource();

            using (var stream = new DataStream(6 * 4 * sizeof(float), true, true))
            {
                stream.Write<Vector4>(new Vector4(0.0f, 0.0f, 1.0f, 1.0f));
                stream.Write<Vector2>(new Vector2(0.0f, 0.0f));

                stream.Write<Vector4>(new Vector4(1.0f, 0.0f, 1.0f, 1.0f));
                stream.Write<Vector2>(new Vector2(1.0f, 0.0f));

                stream.Write<Vector4>(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                stream.Write<Vector2>(new Vector2(1.0f, 1.0f));

                stream.Write<Vector4>(new Vector4(0.0f, 1.0f, 1.0f, 1.0f));
                stream.Write<Vector2>(new Vector2(0.0f, 1.0f));

                stream.Position = 0;
                

                res.buffer = new SlimDX.Direct3D10.Buffer(renderer.device, stream, new BufferDescription()
                {
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    SizeInBytes = 6 * 4 * sizeof(float),
                    Usage = ResourceUsage.Default
                });
            }
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            ((MeshResource)resource).buffer.Dispose();
        }

        public override string Type
        {
            get { return "mesh"; }
        }
    }
}
