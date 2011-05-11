using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement;
using ResourceManagement.Resources;

namespace Graphics
{
    public class ResourceList<T> : IDisposable, IEnumerable<KeyValuePair<string, T>>
        where T : AResource
    {
        List<KeyValuePair<string, T>> resources;

        public ResourceList(List<KeyValuePair<string, ResourceHandle>> handles)
        {
            resources = handles.ConvertAll<KeyValuePair<string, T>>(convert);
        }

        private KeyValuePair<string, T> convert(KeyValuePair<string, ResourceHandle> handle)
        {
            return new KeyValuePair<string, T>(handle.Key, (dynamic)handle.Value.Acquire());
        }


        public dynamic this[int index]
        {
            get
            {
                return resources[index];
            }
        }

        bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                foreach (var resource in resources)
                {
                    resource.Value.Dispose();
                }

                resources = null;
            }
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return resources.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return resources.GetEnumerator();
        }
    }
}
