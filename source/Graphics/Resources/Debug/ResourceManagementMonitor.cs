using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graphics.Resources.Debug
{
    public partial class ResourceManagementMonitor : Form
    {
        protected Dictionary<string, Dictionary<string, ResourceHandle>> resources;

        public ResourceManagementMonitor(Dictionary<string, Dictionary<string, ResourceHandle>> resources)
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
            lock (resources)
            {
                this.data.DataSource = (from a in this.resources
                                        from b in a.Value
                                        select test(a, b)
                                    ).ToArray();
            }
        }

        public struct Output
        { 
            public string type{get; set;}
            public string key{get; set;}
            public string name{get; set;}
            public int activeSlot { get; set; }
            public ResourceState resourceState1{get; set;}
            public ResourceState resourceState2{get; set;}
            public AResource resource1{get; set;}
            public AResource resource2 { get; set; }
        }

        private Output test(KeyValuePair<string, Dictionary<string, ResourceHandle>> a, KeyValuePair<string, ResourceHandle> b)
        {
            using (var blub = b.Value.DebugAcquire())
            {
                return new Output
                {
                    type = a.Key,
                    key = b.Key,
                    name = b.Value.Name,
                    activeSlot = b.Value.ActiveSlot,
                    resourceState1 = b.Value.resources[0].state,
                    resourceState2 = b.Value.resources[1].state,
                    resource1 = b.Value.resources[0].resource,
                    resource2 = b.Value.resources[1].resource
                };
            }

        }
    }
}
