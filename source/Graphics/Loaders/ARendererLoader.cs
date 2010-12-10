using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SlimDX.Direct3D10;
using ResourceManagement;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using Graphics;

namespace Graphics.Loaders
{
    public abstract class ARendererLoader<ResType, Intermediate> : IResourceLoader, IDisposable
    {
        protected Renderer renderer;
        ResourceHandle defaultResource;

        public ARendererLoader(Renderer renderer)
        {
            this.renderer = renderer;
        }

        abstract protected ResType ReadResourceWithName(string name, out Intermediate data);

        abstract protected AResource doLoad(ResType res, Intermediate data);

        abstract protected void doUnload(AResource resource);

        private void loadDefault()
        {
            defaultResource = new ResourceHandle("default", this);
            IEvent evt = new BasicEvent();
            defaultResource.Load(evt);
            EventState state = evt.Wait();
            if (state == EventState.Failed)
                throw new NotSupportedException("Default Resource was not loaded properly");
        }

        private void RendererLoad(ResourceHandle handle, ResType res, Intermediate data)
        {
            handle.inactive.resource = doLoad(res, data);
            handle.inactive.state = ResourceState.Ready;

            handle.Swap();
        }

        private void RendererUnload(ResourceHandle handle)
        {
            doUnload(handle.inactive.resource);

            handle.inactive.resource = null;
            handle.inactive.state = ResourceState.Empty;
        }

        #region IResourceLoader Members

        abstract public string Type { get; }

        public ResourceHandle Default
        {
            get 
            {
                if (defaultResource == null)
                {
                    loadDefault();
                }

                return defaultResource;
            }
        }

        #endregion

        #region ILoader Members

        public void Load(ResourceHandle handle)
        {
            try
            {
                Intermediate data;
                ResType res = ReadResourceWithName(handle.Name, out data);
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        RendererLoad(handle, res, data);
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
                Intermediate data;
                ResType res = ReadResourceWithName(handle.Name, out data);
                renderer.commandQueue.Add(() =>
                {
                    try
                    {
                        try
                        {
                            RendererLoad(handle, res, data);
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
            catch (Exception)
            {
                handle.Finished();
                evt.Abort();
                throw;
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
            catch (Exception)
            {
                handle.Finished();
                throw;
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
            catch (Exception)
            {
                handle.Finished();
                evt.Abort();
                throw;
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
            catch (Exception)
            {
                handle.Finished();
                throw;
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
            catch (Exception)
            {
                handle.Finished();
                evt.Abort();
                throw;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (Default != null)
            {
                IEvent evt = new BasicEvent();

                renderer.commandQueue.Add(() =>
                {
                    defaultResource.Unload(evt);
                    defaultResource = null;
                });
                evt.Wait();
            }
        }

        #endregion
    }
}
