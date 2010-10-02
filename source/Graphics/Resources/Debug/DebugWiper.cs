using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics.Resources.Debug
{
    public class DebugWiper : AWiper
    {
        ResourceManagementMonitor monitor;

        public override void Start()
        {
            monitor = new ResourceManagementMonitor(this.resources);
            monitor.Show();
        }

        public override void Stop()
        {
            monitor.Close();
        }
    }
}
