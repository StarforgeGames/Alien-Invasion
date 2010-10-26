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
    public abstract class ARendererLoader<T> : IResourceLoader, IDisposable
    {
        protected Renderer renderer;
        ResourceHandle defaultResource;

        public ARendererLoader(Renderer renderer)
        {
            this.renderer = renderer;
        }

        abstract protected T ReadResourceWithName(string name);

        abstract protected AResource doLoad(T data);

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

        private void RendererLoad(ResourceHandle handle, T data)
        {
            handle.inactive.resource = doLoad(data);
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
                T data = ReadResourceWithName(handle.Name);
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
                T data = ReadResourceWithName(handle.Name);
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
                    catch (Exception e)
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
