using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using SlimDX.Direct3D10;
using System.IO;
using Graphics.Resources;

namespace Graphics.Loaders
{
    public class TextureLoader : ARendererLoader<byte[]>, IFileLoader
    {
        public ResourceNameConverter Converter
        {
            get { return converter; }
        }
        
        private readonly ResourceNameConverter converter = 
            new ResourceNameConverter(@"data\sprites\", @".png");

        protected override byte[] ReadResourceWithName(string name)
        {
            return File.ReadAllBytes(converter.getFilenameFrom(name));
        }

        protected override AResource doLoad(byte[] data)
        {
            TextureResource res = new TextureResource();
            res.texture = ShaderResourceView.FromMemory(renderer.device, data);
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            ((TextureResource)resource).texture.Dispose();
        }

        public TextureLoader(Renderer renderer) : base(renderer)
        {
        }

        public override string Type
        {
            get { return "texture"; }
        }

    }
}
