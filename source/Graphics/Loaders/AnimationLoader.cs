using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.Resources;

namespace Graphics.Loaders
{
    public class AnimationLoader : TextureLoader
    {
        public AnimationLoader(Renderer renderer) : base(renderer)
        {
        }

        public override string Type
        {
            get
            {
                return base.Type;
            }
        }

        protected override Resources.TextureResource ReadResourceWithName(string name, out byte[] data)
        {
            AnimationResource res = new AnimationResource();
            
            throw new NotImplementedException();
            //return 
        }
    }
}
