using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;
using ResourceManagement.Resources;

namespace Graphics.Resources
{
    public class TextureResource : AResource
    {
        public ShaderResourceView texture;

        public TextureResource()
        {
            //new EffectVariableDescription().Semantic
            //Effect.FromPointer(null).GetVariableBySemantic("")
            //Effect.FromPointer(null).GetVariableByName("").AsResource().
            
        }
    }
}
