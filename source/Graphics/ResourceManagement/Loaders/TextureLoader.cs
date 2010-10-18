﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement.Resources;
using SlimDX.Direct3D10;
using System.IO;

namespace Graphics.ResourceManagement.Loaders
{
    public class TextureLoader : ARendererLoader<byte[]>
    {
        string baseDirectory = @"Gfx\";
        string extension = @".png";

        protected override byte[] ReadResourceWithName(string name)
        {
            return File.ReadAllBytes(baseDirectory + name + extension);
        }

        protected override AResource doLoad(byte[] data)
        {
            TextureResource tex = new TextureResource();
            tex.texture = ShaderResourceView.FromMemory(renderer.device, data);
            return tex;
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