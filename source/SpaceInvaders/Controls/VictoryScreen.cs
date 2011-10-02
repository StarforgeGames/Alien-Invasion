using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.EventManagement;

namespace SpaceInvaders.Controls
{
    public partial class VictoryScreen : UserControl
    {
        private IEventManager eventManager;

        public VictoryScreen(IEventManager eventManager)
        {
            this.eventManager = eventManager;

            InitializeComponent();
        }
    }
}
