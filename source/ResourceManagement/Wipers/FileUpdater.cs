using System;
using System.Collections.Concurrent;
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
				loader = (from l in manager.Loaders.Values
							where l is IFileLoader
							where ((IFileLoader)l).Converter.isResourceFor(e.Name)
							select l).FirstOrDefault();
				
				if (loader == null)
				{
					return;
				}

				string resourceName = ((IFileLoader)loader).Converter.getResourceNameFrom(e.Name);

				ConcurrentDictionary<string, ResourceHandle> resourcesOfType;

				if (resources.TryGetValue(loader.Type, out resourcesOfType))
				{
					ResourceHandle handle;
					if (resourcesOfType.TryGetValue(resourceName, out handle))
					{
						if (handle.active.state == ResourceState.Ready)
						{
							handle.Load();
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
