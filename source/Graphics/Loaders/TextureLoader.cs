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
    public class TextureLoader : ASingleThreadedLoader<TextureResource, byte[]>, IFileLoader
    {
        Renderer renderer;

        public ResourceNameConverter Converter
        {
            get { return converter; }
        }
        
        private readonly ResourceNameConverter converter = 
            new ResourceNameConverter(@"data\sprites\", @".png");

        protected override TextureResource ReadResourceWithName(string name, out byte[] data)
        {
            data =  File.ReadAllBytes(converter.getFilenameFrom(name));
            return new TextureResource();
        }

        protected override AResource doLoad(TextureResource res, byte[] data)
        {
            res.texture = ShaderResourceView.FromMemory(renderer.device, data);
            return res;
        }

        protected override void doUnload(AResource resource)
        {
            ((TextureResource)resource).texture.Dispose();
        }

        public TextureLoader(Renderer renderer) : base(renderer.commandQueue)
        {
            this.renderer = renderer;
        }

        public override string Type
        {
            get { return "texture"; }
        }
    }
}
