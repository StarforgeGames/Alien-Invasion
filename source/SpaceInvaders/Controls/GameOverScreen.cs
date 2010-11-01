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
    public partial class GameOverScreen : UserControl
    {
        private IEventManager eventManager;

        public GameOverScreen(IEventManager eventManager)
        {
            this.eventManager = eventManager;

            InitializeComponent();
        }
    }
}
