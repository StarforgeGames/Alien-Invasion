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
using SlimDX.DXGI;
using LispInterpreter;
using Graphics.Loaders.Mesh;

namespace Graphics.Loaders
{
    public class MeshLoader : ARendererLoader<MeshResource, bool>, IFileLoader
    {
        Interpreter interpreter = new Interpreter();
                
        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\meshes\", @".mesh");

        public MeshLoader(Renderer renderer) : base(renderer)
        {
            interpreter.Load(typeof(MeshBuiltins));
        }

        protected override MeshResource ReadResourceWithName(string name, out bool data)
        {
            using (var file = File.Open(converter.getFilenameFrom(name), FileMode.Open))
            {
                var globalEnvironment = interpreter.createEnvironment();
                MeshResource resource = interpreter.Eval(file, globalEnvironment);
                data = false;
                return resource;
            }
        }

        protected override AResource doLoad(MeshResource res, bool data)
        {
            res.buffer = new SlimDX.Direct3D10.Buffer(renderer.device, res.stream, new BufferDescription()
            {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                SizeInBytes = res.elementSize * res.elementCount,
                Usage = ResourceUsage.Default
            });

            res.stream.Dispose();
            res.stream = null;
         
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
