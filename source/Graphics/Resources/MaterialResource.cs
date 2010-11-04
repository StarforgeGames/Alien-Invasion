using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Resources;
using SlimDX.Direct3D10;

namespace Graphics.Resources
{
    public class MaterialResource : AResource
    {
        public EffectResource effect = null;
        public TextureResource texture = null;
        
        ResourceHandle effectHandle;
        ResourceHandle textureHandle;

        public MaterialResource(ResourceHandle effectHandle, ResourceHandle textureHandle)
        {
            this.effectHandle = effectHandle;
            this.textureHandle = textureHandle;
        }

        public override void Acquire()
        {
            base.Acquire();
            effect = (EffectResource)effectHandle.Acquire();
            texture = (TextureResource)textureHandle.Acquire();
        }

        public override void Dispose()
        {
            effect.Dispose();
            texture.Dispose();
            base.Dispose();
        }
    }
}
