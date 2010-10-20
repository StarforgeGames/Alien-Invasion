using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Graphics.ResourceManagement.Wipers;

namespace Graphics.ResourceManagement.Debug
{
    public class DebugWiper : AWiper
    {
        ResourceManagementMonitor monitor;
        Thread[] timers = new Thread[0];
        
        bool running = true;

        public override void Start()
        {
            monitor = new ResourceManagementMonitor(this.resources);
            monitor.Show();
            for (int i = 0; i < timers.Length; ++i )
            {
                running = true;
                timers[i] = new Thread(t_Elapsed);
                timers[i].Name = "Debug " + i;
                timers[i].Start();

            }
        }

        public override void Stop()
        {

            running = false;

            monitor.Close();
            foreach (var timer in timers)
            {
                timer.Join();
            }
        }
        public DebugWiper()
        {
        }

        [System.ThreadStatic]
        static int time;

        void t_Elapsed()
        {
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
            time = rand.Next(5000, 10000);
            while (running)
            {
                int index1 = rand.Next(resources.Count);
                lock (resources) // suboptimale lösung. wiper sollten keine exklusive Locks auf die resourcen setzen, da sie nur lesend zugreifen
                {
                    var currentResourceType = resources.ElementAt(index1).Value;
                    lock (currentResourceType)
                    {
                        if (currentResourceType.Count > 0)
                        {
                            int index2 = rand.Next(currentResourceType.Count);
                            int operation = rand.Next(2);
                            var currentResource = currentResourceType.ElementAt(index2).Value;
                            switch (operation)
                            {
                                case 0:

                                    currentResource.Load();
                                    break;
                                case 1:

                                    currentResource.Unload();
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (rand.Next(2) == 1)
                        {
                            int i = rand.Next(10);
                            manager.GetResource("blub" + i, "txt");
                        }
                    }
                }
                System.Threading.Thread.Sleep(time);
            }
            
        }   
    }
}
