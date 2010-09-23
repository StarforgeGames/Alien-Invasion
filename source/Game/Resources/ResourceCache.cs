using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Resources
{

    public class ResourceCache
    {
        public int id;

        Dictionary<int, Resource> resourcesByID;
        Dictionary<string, Resource> resourcesByName;

        public ResourceCache()
        {
            id = 0;
            resourcesByID = new Dictionary<int, Resource>();
            resourcesByName = new Dictionary<string, Resource>();
        }

        public Resource GetResource(int id)
        {
            return resourcesByID[id];
        }

        public Resource GetResource(string name)
        {
            if (!resourcesByName.ContainsKey(name)) {
                Load(name);
            }

            return resourcesByName[name];
        }

        public void Load(string filepath)
        {
            Resource res = new Resource(++id, filepath);
           
            resourcesByID.Add(res.ID, res);
            resourcesByName.Add(res.Name, res);
        }
    }

}
