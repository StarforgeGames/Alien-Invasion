using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX.Direct3D10;
using SlimDX.D3DCompiler;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
{
    public class EffectLoader : ARendererLoader<byte[]>
    {
        string baseDirectory = @"Shaders\";
        string extension = @".fx";

        public EffectLoader(Renderer renderer) : base(renderer)
        {
        }

        protected override byte[] ReadResourceWithName(string name)
        {
            return File.ReadAllBytes(baseDirectory + name + extension);
        }

        protected override Resources.AResource doLoad(byte[] data)
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

        protected override void doUnload(Resources.AResource resource)
        {
            ((EffectResource)resource).effect.Dispose();
        }

        public override string Type
        {
            get { return "fx"; }
        }
    }
}
