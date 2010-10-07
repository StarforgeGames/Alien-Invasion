using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;

namespace Graphics.ResourceManagement.Resources
{
    public class TextureResource : AResource
    {
        private ShaderResourceView texture;

        public TextureResource(ShaderResourceView texture)
        {
            this.texture = texture;
        }
    }
}
