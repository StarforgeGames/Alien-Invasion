using System;
using Graphics.Resources;
using ResourceManagement.Resources;
using ResourceManagement.Loaders;
using ResourceManagement;

namespace Graphics.Loaders
{
    public class AnimationLoader : ABasicLoader, IFileLoader
    {
        private ResourceManager manager;

        public AnimationLoader(ResourceManager manager)
        {
            this.manager = manager;
        }

        public override string Type
        {
            get { return "animation"; }
        }

        protected override AResource doLoad(string name)
        {

            var res = new AnimationResource(manager.GetResource("dummy", "texture")); // todo load correct texture
            
            throw new NotImplementedException();
        }

        protected override void doUnload(AResource resource)
        {
            // do nothing since the animation resource does not consume much memory and will be collected by the gc.
        }

        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\animations\", @".animation");
    }
}
