using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Graphics.Resources.Debug
{
    public class DebugWiper : AWiper
    {
        ResourceManagementMonitor monitor;
        Thread[] timers = new Thread[20];
        
        bool running = true;

        public override void Start()
        {
            monitor = new ResourceManagementMonitor(this.resources);
            monitor.Show();
            for (int i = 0; i < timers.Length; ++i )
            {
                running = true;
                timers[i] = new Thread(t_Elapsed);
                timers[i].Start();

            }
        }

        public override void Stop()
        {

            running = false;

            monitor.Close();
        }
        public DebugWiper()
        {
        }

        [System.ThreadStatic]
        static int time;

        void t_Elapsed()
        {
            Random rand = new Random(Thread.CurrentThread.ManagedThreadId);
            time = rand.Next(50, 200);
            while (running)
            {
                int index1 = rand.Next(resources.Count);
                var currentResource = resources.ElementAt(index1).Value;
                int index2 = rand.Next(currentResource.Count);
                int operation = rand.Next(2);
                switch (operation)
                {
                    case 0:

                        currentResource.ElementAt(index2).Value.Load();
                        break;
                    case 1:
                    default:
                        currentResource.ElementAt(index2).Value.Unload();
                        break;
                }
                System.Threading.Thread.Sleep(time);
            }
            
        }   
    }
}
