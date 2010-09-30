using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Graphics.Resources
{
    public enum ResourceState
    {
        Ready, Loading, Unloading, Empty
    }

    public class ResourceHandle
    {
        internal ResourceProperties[] resources = new ResourceProperties[2];
        private IResourceLoader resourceLoader;
        private int slot = 0;
        private int pendingSlot = 0;
        private int pendingOperation = 0;
        private string name;

        public ResourceHandle(string name, IResourceLoader resourceLoader)
        {
            this.name = name;
            this.resourceLoader = resourceLoader;
        }

        internal int ActiveSlot
        {
            get
            {
                return slot;
            }
        }

        internal int InactiveSlot
        {
            get
            {
                return slot == 0 ? 1 : 0;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public AResource Acquire()
        {
            while (Interlocked.CompareExchange(ref pendingSlot, 1, 0) != 0) ;
            try
            {
                switch (resources[ActiveSlot].status)
                {
                    case ResourceState.Ready:
                        resources[ActiveSlot].resource.Acquire();
                        return resources[ActiveSlot].resource;

                    case ResourceState.Empty:
                        Load();
                        resourceLoader.Default.Acquire();
                        return resourceLoader.Default;

                    case ResourceState.Loading:
                        resourceLoader.Default.Acquire();
                        return resourceLoader.Default;

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

        public void Load()
        {
            if ((Interlocked.CompareExchange(ref pendingOperation, 1, 0) == 0))
            {
                switch (resources[InactiveSlot].status)
                {
                    case ResourceState.Empty:
                        resourceLoader.Load(this);
                        return;
                    case ResourceState.Ready:
                        resourceLoader.Reload(this);
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

            switch (resources[InactiveSlot].status)
            {
                case ResourceState.Empty:
                    resourceLoader.Load(this, evt);
                    return;
                case ResourceState.Ready:
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
                switch (resources[InactiveSlot].status)
                {
                    case ResourceState.Ready:
                        resourceLoader.Unload(this);
                        return;
                    case ResourceState.Empty:
                        switch (resources[ActiveSlot].status)
                        {
                            case ResourceState.Ready:
                                Swap();
                                resourceLoader.Unload(this);
                                return;
                            case ResourceState.Empty:
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

            switch (resources[InactiveSlot].status)
            {
                case ResourceState.Ready:
                    resourceLoader.Unload(this, evt);
                    return;
                case ResourceState.Empty:
                    switch (resources[ActiveSlot].status)
                    {
                        case ResourceState.Ready:
                            Swap();
                            resourceLoader.Unload(this, evt);
                            return;
                        case ResourceState.Empty:
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
            slot = slot == 1 ? 0 : 1;
            Interlocked.Decrement(ref pendingSlot);
        }

        public void Finished()
        {
            Interlocked.Decrement(ref pendingOperation);
        }
    }
}
