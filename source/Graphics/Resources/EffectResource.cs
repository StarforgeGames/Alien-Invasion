using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D10;
using ResourceManagement.Resources;

namespace Graphics.Resources
{
    public class EffectResource : AResource
    {
        public string errors;
        public Effect Value;
    }
}
