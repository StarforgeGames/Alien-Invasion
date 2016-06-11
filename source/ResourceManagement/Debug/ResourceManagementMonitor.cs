using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ResourceManagement.Resources;

namespace ResourceManagement.Debug
{
    public partial class ResourceManagementMonitor : Form
    {
        protected ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>> resources;

        public ResourceManagementMonitor(ConcurrentDictionary<string, ConcurrentDictionary<string, ResourceHandle>> resources)
        {
            this.resources = resources;
            InitializeComponent();
        }

        private void data_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
            else
            {
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            object source;
            
            source = (from a in this.resources
                                    from b in a.Value
                                    select test(a, b)
                                ).ToArray();
            
            this.data.DataSource = source;
        }

        public struct Output
        { 
            public string type{get; set;}
            public string key{get; set;}
            public string name{get; set;}
            public ResourceState activeState { get; set; }
            public AResource active { get; set; }

            public ResourceState inactiveState { get; set; }
            public AResource inactive { get; set; }
        }

        private Output test(KeyValuePair<string, ConcurrentDictionary<string, ResourceHandle>> a, KeyValuePair<string, ResourceHandle> b)
        {
            using (var blub = b.Value.DebugAcquire())
            {
                return new Output
                {
                    type = a.Key,
                    key = b.Key,
                    name = b.Value.Name,
                    activeState = b.Value.active.state,
                    inactiveState = b.Value.inactive.state,
                    active = b.Value.active.resource,
                    inactive = b.Value.inactive.resource
                };
            }

        }
    }
}
