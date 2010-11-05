using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX.Direct3D10;
using SlimDX.D3DCompiler;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using Graphics.Resources;

namespace Graphics.Loaders
{
    public class EffectLoader : ARendererLoader<byte[]>, IFileLoader
    {
        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\shaders\", @".fx");

        public EffectLoader(Renderer renderer) : base(renderer)
        {
        }

        protected override byte[] ReadResourceWithName(string name)
        {
            return File.ReadAllBytes(converter.getFilenameFrom(name));
        }

        protected override AResource doLoad(byte[] data)
        {
            EffectResource res = new EffectResource();

            res.effect = Effect.FromMemory(
                device: renderer.device,
                memory: data, 
                profile: "fx_4_0", 
                shaderFlags: ShaderFlags.None,
                effectFlags: EffectFlags.SingleThreaded,
                pool: null,
                include: null,
                compilationErrors: out res.errors);

            return res;
        }

        protected override void doUnload(AResource resource)
        {
            ((EffectResource)resource).effect.Dispose();
        }

        public override string Type
        {
            get { return "effect"; }
        }
    }
}
