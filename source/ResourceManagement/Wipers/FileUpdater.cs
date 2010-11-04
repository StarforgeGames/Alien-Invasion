using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ResourceManagement.Loaders;

namespace ResourceManagement.Wipers
{
    public class FileUpdater : AWiper
    {
        FileSystemWatcher watcher = new FileSystemWatcher();
        FileSystemEventArgs oldArgs;
        bool toggle = false;

        public override void Start()
        {
            watcher.Path = "./";
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "";
            
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (toggle)
            {
                
                System.Console.WriteLine("File changed: '" + e.Name + "', change type: '" + e.ChangeType + "'");

                IResourceLoader loader;
                lock (manager.Loaders)
                {
                    loader = (from l in manager.Loaders.Values
                                  where l is IFileLoader
                                  where ((IFileLoader)l).Converter.isResourceFor(e.Name)
                                  select l).First();
                }
                string resourceName = ((IFileLoader)loader).Converter.getResourceNameFrom(e.Name);

                lock (resources)
                {
                    var resourceType = resources[loader.Type];
                    lock (resourceType)
                    {
                        if (resourceType.ContainsKey(resourceName))
                        {
                            ResourceHandle res = resourceType[resourceName];
                            if(res.active.state == ResourceState.Ready)
                            {
                                res.Load();
                            }
                        }
                    }
                }
                
            }
            toggle = !toggle;
            oldArgs = e;
        }

        public override void Stop()
        {
            watcher.EnableRaisingEvents = false;
        }
    }
}
