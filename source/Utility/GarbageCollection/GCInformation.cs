using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;

namespace Utility.GarbageCollection
{
    public class GCInformation
    {
        Thread thread;
        bool isRunning;

        public int Interval { get; set; }

        public readonly int[] totalCollections;
        public readonly int[] collectionsInInterval;

        public long TotalMemory;

        public GCInformation()
        {
            Interval = 1000;

            isRunning = true;
            thread = new Thread(loop);

            totalCollections = new int[GC.MaxGeneration];
            collectionsInInterval = new int[GC.MaxGeneration];
            thread.Start();
        }

        private void loop()
        {
            while (isRunning)
            {
                for(int i = 0; i < GC.MaxGeneration; ++i)
                {
                    int count = GC.CollectionCount(i);
                    collectionsInInterval[i] = count - totalCollections[i];
                    totalCollections[i] = count;
                }
                
                TotalMemory = GC.GetTotalMemory(false);
                Thread.Sleep(Interval);
                Console.WriteLine("TotalMem: " + TotalMemory);
                for (int i = 0; i < GC.MaxGeneration; ++i)
                {
                    Console.WriteLine("#Gen" + i + ": " + totalCollections[i]);
                }
            }
        }

        ~GCInformation()
        {
            isRunning = false;
            Interval = 0;
            thread.Join();
        }
    }
}
