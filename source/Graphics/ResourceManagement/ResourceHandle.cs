using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Graphics.ResourceManagement.Resources;
using Graphics.ResourceManagement.Loaders;

namespace Graphics.ResourceManagement
{
    public enum ResourceState
    {
        Ready, Loading, Unloading, Empty
    }

    public class ResourceHandle
    {
        internal ResourceProperties 
            active = new ResourceProperties {
                resource = null,
                state = ResourceState.Empty
            },
            inactive = new ResourceProperties()
            {
                resource = null,
                state = ResourceState.Empty
            };

        
        private IResourceLoader resourceLoader;
        private int pendingSlot = 0;
        private int pendingOperation = 0;

        public ResourceHandle(string name, IResourceLoader resourceLoader)
        {
            this.Name = name;
            this.resourceLoader = resourceLoader;
        }

        public string Name
        {
            get;
            private set;
        }

        internal AResource DebugAcquire()
        {
            while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0) ;
            try
            {
                switch (active.state)
                {
                    case ResourceState.Ready:
                        active.resource.Acquire();
                        return active.resource;

                    case ResourceState.Empty:
                        return resourceLoader.Default.Acquire();

                    case ResourceState.Loading:
                        return resourceLoader.Default.Acquire();

                    case ResourceState.Unloading:
                    default:
                        throw new NotSupportedException("Tried to acquire resource that is unloading.");
                }
            }
            finally
            {
                Interlocked.Decrement(ref pendingSlot);
            }
        }

        public AResource Acquire()
        {
            while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0) ;
            try
            {
                switch (active.state)
                {
                    case ResourceState.Ready:
                        active.resource.Acquire();
                        AResource result = active.resource;
                        Interlocked.Decrement(ref pendingSlot);
                        return result;

                    case ResourceState.Empty:
                        Interlocked.Decrement(ref pendingSlot);
                        Load();
                        return resourceLoader.Default.Acquire();

                    case ResourceState.Loading:
                        Interlocked.Decrement(ref pendingSlot);
                        return resourceLoader.Default.Acquire();

                    case ResourceState.Unloading:
                    default:
                        throw new NotSupportedException("Tried to acquire resource that is unloading.");
                }
            }
            catch (Exception e)
            {
                Interlocked.Decrement(ref pendingSlot);
                throw e;
            }
        }

        public void Load()
        {
            if ((Interlocked.CompareExchange(ref pendingOperation, 1, 0) == 0))
            {
                switch (inactive.state)
                {
                    case ResourceState.Empty:
                        inactive.state = ResourceState.Loading;
                        resourceLoader.Load(this);
                        return;
                    case ResourceState.Ready:
                        if (inactive.resource.IsAcquired)
                        {
                            Interlocked.Decrement(ref pendingOperation);
                        }
                        else
                        {
                            inactive.state = ResourceState.Unloading;
                            resourceLoader.Reload(this);
                        }
                        return;
                    case ResourceState.Loading:
                        throw new NotSupportedException("tried to load resource while loading");
                    case ResourceState.Unloading:
                        throw new NotSupportedException("tried to load resource while unloading");
                    default:
                        throw new NotSupportedException("this should never occur!");
                }
            }
        }

        public void Load(IEvent evt)
        {
            while ((Interlocked.CompareExchange(ref pendingOperation, 1, 0) == 1));

            switch (inactive.state)
            {
                case ResourceState.Empty:
                    resourceLoader.Load(this, evt);
                    return;
                case ResourceState.Ready:
                    while (inactive.resource.IsAcquired) ;
                    resourceLoader.Reload(this, evt);
                    return;
                case ResourceState.Loading:
                    throw new NotSupportedException("tried to load resource while loading");
                case ResourceState.Unloading:
                    throw new NotSupportedException("tried to load resource while unloading");
                default:
                    throw new NotSupportedException("this should never occur!");
            }
        }

        public void Unload()
        {
            if ((Interlocked.CompareExchange(ref pendingOperation, 1, 0) == 0))
            {
                switch (inactive.state)
                {
                    case ResourceState.Ready:
                        if (inactive.resource.IsAcquired)
                        {
                            Interlocked.Decrement(ref pendingOperation);
                        }
                        else
                        {
                            inactive.state = ResourceState.Unloading;
                            resourceLoader.Unload(this);
                        }
                        return;
                        
                    case ResourceState.Empty:
                        switch (active.state)
                        {
                            case ResourceState.Ready:
                                bool swapped = false;
                                while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0) ;
                                if (!active.resource.IsAcquired)
                                {
                                    ResourceProperties temp = active;
                                    active = inactive;
                                    inactive = temp;
                                    swapped = true;
                                }
                                Interlocked.Decrement(ref pendingSlot);
                                if (swapped)
                                {
                                    inactive.state = ResourceState.Unloading;
                                    resourceLoader.Unload(this);
                                }
                                else
                                {
                                    Interlocked.Decrement(ref pendingOperation);
                                }
                                return;
                            case ResourceState.Empty:
                                Interlocked.Decrement(ref pendingOperation);
                                return;
                            case ResourceState.Loading:
                                throw new NotSupportedException("tried to unload resource while loading");
                            case ResourceState.Unloading:
                                throw new NotSupportedException("tried to unload resource while unloading");
                            
                            default:
                                throw new NotSupportedException("this should never occur!");
                        }

                    case ResourceState.Loading:
                        throw new NotSupportedException("tried to unload resource while loading");
                    case ResourceState.Unloading:
                        throw new NotSupportedException("tried to unload resource while unloading");
                    default:
                        throw new NotSupportedException("this should never occur!");
                }
            }
        }

        public void Unload(IEvent evt)
        {
            while ((Interlocked.CompareExchange(ref pendingOperation, 1, 0) == 1)) ;

            switch (inactive.state)
            {
                case ResourceState.Ready:
                    while (inactive.resource.IsAcquired) ;
                    inactive.state = ResourceState.Unloading;
                    resourceLoader.Unload(this, evt);
                    return;
                case ResourceState.Empty:
                    switch (active.state)
                    {
                        case ResourceState.Ready:
                            while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0) ;
                            while (active.resource.IsAcquired) ;
                            ResourceProperties temp = active;
                            active = inactive;
                            inactive = temp;
                            Interlocked.Decrement(ref pendingSlot);
                            inactive.state = ResourceState.Unloading;
                            resourceLoader.Unload(this, evt);
                            
                            return;
                        case ResourceState.Empty:
                            Interlocked.Decrement(ref pendingOperation);
                            evt.Finish();
                            return;
                        case ResourceState.Loading:
                            throw new NotSupportedException("tried to unload resource while loading");
                        case ResourceState.Unloading:
                            throw new NotSupportedException("tried to unload resource while unloading");
                        default:
                            throw new NotSupportedException("this should never occur!");
                    }

                case ResourceState.Loading:
                    throw new NotSupportedException("tried to unload resource while loading");
                case ResourceState.Unloading:
                    throw new NotSupportedException("tried to unload resource while unloading");
                default:
                    throw new NotSupportedException("this should never occur!");
            }
        }

        public void Swap()
        {
            while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0);
            //inactive = Interlocked.Exchange(ref active, inactive);
            ResourceProperties temp = active;
            active = inactive;
            inactive = temp;
            Interlocked.Decrement(ref pendingSlot);
        }

        public void Finished()
        {
            Interlocked.Decrement(ref pendingOperation);
        }
    }
}
