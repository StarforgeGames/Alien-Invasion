using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX.Direct3D10;
using Graphics.ResourceManagement.Resources;

namespace Graphics.ResourceManagement.Loaders
{
    class TextureLoader : IResourceLoader
    {
        Renderer renderer;
        string baseDirectory = @"Gfx\";

        TextureLoader(Renderer renderer)
        {
            this.renderer = renderer;
        }

        #region IResourceLoader Members

        public string Type
        {
            get { return "texture"; }
        }

        public Resources.AResource Default
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ILoader Members

        public void Load(ResourceHandle resourceHandle)
        {
            try
            {
                byte[] image = File.ReadAllBytes(baseDirectory + resourceHandle.Name + ".png");
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        resourceHandle.inactive.resource = new TextureResource(renderer, image);
                        resourceHandle.inactive.state = ResourceState.Ready;
                        resourceHandle.Swap();
                    }
                    finally
                    {
                        resourceHandle.Finished();
                    }
                });
            }
            catch (Exception e)
            {
                resourceHandle.Finished();
                throw e;
            }
            
        }

        public void Load(ResourceHandle resourceHandle, IEvent evt)
        {
            try
            {
                byte[] image = File.ReadAllBytes(baseDirectory + resourceHandle.Name + ".png");
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        try
                        {
                            resourceHandle.inactive.resource = new TextureResource(renderer, image);
                            resourceHandle.inactive.state = ResourceState.Ready;
                            resourceHandle.Swap();
                        }
                        finally
                        {
                            resourceHandle.Finished();
                        }
                        evt.Finish();
                    }
                    catch (Exception)
                    {
                        evt.Abort();
                    }
                });
            }
            catch (Exception e)
            {
                resourceHandle.Finished();
                evt.Abort();
                throw e;
            }
        }

        public void Reload(ResourceHandle resourceHandle)
        {
            throw new NotImplementedException();
        }

        public void Reload(ResourceHandle resourceHandle, IEvent evt)
        {
            throw new NotImplementedException();
        }

        public void Unload(ResourceHandle resourceHandle)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        TextureResource tex = (TextureResource)resourceHandle.inactive.resource;
                        resourceHandle.inactive.resource.Dispose();
                        resourceHandle.inactive.resource = null;
                        resourceHandle.inactive.state = ResourceState.Empty;
                        resourceHandle.Swap();
                    }
                    finally
                    {
                        resourceHandle.Finished();
                    }
                });
            }
            catch (Exception e)
            {
                resourceHandle.Finished();
                throw e;
            }
        }

        public void Unload(ResourceHandle resourceHandle, IEvent evt)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        try
                        {
                            TextureResource tex = (TextureResource)resourceHandle.inactive.resource;
                            resourceHandle.inactive.resource.Dispose();
                            resourceHandle.inactive.resource = null;
                            resourceHandle.inactive.state = ResourceState.Empty;
                            resourceHandle.Swap();
                        }
                        finally
                        {
                            resourceHandle.Finished();
                        }
                        evt.Finish();
                    }
                    catch (Exception)
                    {
                        evt.Abort();
                    }
                    
                });
            }
            catch (Exception e)
            {
                resourceHandle.Finished();
                evt.Abort();
                throw e;
            }
        }

        #endregion
    }
}
