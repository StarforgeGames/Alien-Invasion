using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Resources;
using ResourceManagement;

namespace Graphics.Resources
{
    public class AnimationResource : AResource
    {
        public int columCount, rowCount;

        private ResourceHandle textureHandle;

        public TextureResource texture;

        public AnimationResource(ResourceHandle textureHandle)
        {
            this.textureHandle = textureHandle;
        }

        public override void Acquire()
        {
            texture = (TextureResource)textureHandle.Acquire();
            base.Acquire();
        }

        public override void Dispose()
        {
            texture.Dispose();
            base.Dispose();
        }
    }
}
