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

        private byte[] ReadFromFile(ResourceHandle handle)
        {
            return File.ReadAllBytes(baseDirectory + handle.Name + ".png");
        }

        private void RendererLoad(ResourceHandle handle, byte[] data)
        {
            TextureResource res = new TextureResource();
            res.texture = ShaderResourceView.FromMemory(renderer.device, data);

            handle.inactive.resource = res;
            handle.inactive.state = ResourceState.Ready;

            handle.Swap();
        }

        private void RendererUnload(ResourceHandle handle)
        {
            TextureResource tex = (TextureResource)handle.inactive.resource;
            handle.inactive.resource = null;
            handle.inactive.state = ResourceState.Empty;
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

        public void Load(ResourceHandle handle)
        {
            try
            {
                byte[] data = ReadFromFile(handle);
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererLoad(handle, data);
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("Exception in commandQueue should be handled.");
                    }
                    finally
                    {
                        handle.Finished();
                    }
                });
            }
            catch (Exception e)
            {
                handle.Finished();
                throw e;
            }
            
        }

        public void Load(ResourceHandle handle, IEvent evt)
        {
            try
            {
                byte[] data = ReadFromFile(handle);
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        try
                        {
                            RendererLoad(handle, data);
                        }
                        finally
                        {
                            handle.Finished();
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
                handle.Finished();
                evt.Abort();
                throw e;
            }
        }

        public void Reload(ResourceHandle handle)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererUnload(handle);
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("Exception in commandQueue should be handled.");
                    }
                });
            }
            catch (Exception e)
            {
                handle.Finished();
                throw e;
            }

            Load(handle);
        }

        public void Reload(ResourceHandle handle, IEvent evt)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererUnload(handle);
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("Exception in commandQueue should be handled.");
                    }
                });
            }
            catch (Exception e)
            {
                handle.Finished();
                evt.Abort();
                throw e;
            }

            Load(handle, evt);
        }

        public void Unload(ResourceHandle handle)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererUnload(handle);
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("Exception in commandQueue should be handled.");
                    }
                    finally
                    {
                        handle.Finished();
                    }
                });
            }
            catch (Exception e)
            {
                handle.Finished();
                throw e;
            }
        }

        public void Unload(ResourceHandle handle, IEvent evt)
        {
            try
            {
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererUnload(handle);
                        evt.Finish();
                    }
                    catch (Exception)
                    {
                        evt.Abort();
                    }
                    finally
                    {
                        handle.Finished();
                    }
                    
                });
            }
            catch (Exception e)
            {
                handle.Finished();
                evt.Abort();
                throw e;
            }
        }

        #endregion
    }
}
