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

namespace Graphics.Loaders.Mesh
{
    public class MeshLoader : ASingleThreadedLoader<MeshResource, bool>, IFileLoader
    {
        Renderer renderer;

        Interpreter interpreter = new Interpreter();
                
        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\meshes\", @".mesh");

        public MeshLoader(Renderer renderer) : base(renderer.commandQueue)
        {
            this.renderer = renderer;
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
            using (res.indexstream)
            {
                using (res.vertexstream)
                {
                    res.vertexBuffer = new SlimDX.Direct3D10.Buffer(renderer.device, res.vertexstream, new BufferDescription()
                    {
                        BindFlags = BindFlags.VertexBuffer,
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None,
                        SizeInBytes = (int)res.vertexstream.Length,
                        Usage = ResourceUsage.Default
                    });
                }

                if (res.indexed)
                {
                    res.indexBuffer = new SlimDX.Direct3D10.Buffer(renderer.device, res.indexstream, new BufferDescription()
                    {
                        BindFlags = BindFlags.IndexBuffer,
                        CpuAccessFlags = CpuAccessFlags.None,
                        OptionFlags = ResourceOptionFlags.None,
                        SizeInBytes = (int)res.indexstream.Length,
                        Usage = ResourceUsage.Default
                    });
                }
            }
         
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            ((MeshResource)resource).vertexBuffer.Dispose();
            ((MeshResource)resource).indexBuffer.Dispose();
        }

        public override string Type
        {
            get { return "mesh"; }
        }
    }
}
