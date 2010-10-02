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
            
            this.data.DataSource = (from a in this.resources
                                    from b in a.Value
                                   select new { type = a.Key, key=b.Key, value = b.Value }).ToArray();
        }
    }
}
