using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Resources;

namespace Graphics.Resources
{
    public class MaterialResource : AResource
    {
        public MaterialResource(ResourceHandle effectHandle, ResourceHandle textureHandle)
        {
            this.effectHandle = effectHandle;
            this.textureHandle = textureHandle;
        }

        ResourceHandle effectHandle, textureHandle;

        public EffectResource effect = null;
        public TextureResource texture = null;

        public override void Acquire()
        {
            base.Acquire();
            effect = (EffectResource)effectHandle.Acquire();
            texture = (TextureResource)textureHandle.Acquire();

        }

        public override void Dispose()
        {
            effect.Dispose();
            effect = null;

            texture.Dispose();
            texture = null;

            base.Dispose();
        }
    }
}
