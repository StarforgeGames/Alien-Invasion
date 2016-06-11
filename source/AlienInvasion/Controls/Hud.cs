using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace AlienInvasion.Controls
{
    public partial class Hud : UserControl, IEventListener
    {
        private IEventManager eventManager;

        public Hud(IEventManager eventManager)
        {
            InitializeComponent();

            this.eventManager = eventManager;
            this.eventManager.AddListener(this, typeof(HudEvent));
        }

        public void Reset(int initialLifes)
        {
            this.lifesValueLabel.Text = initialLifes.ToString();
            this.scoreValueLabel.Text = "0";
        }

        public void OnEvent(Event evt)
        {
            switch (evt.Type)
            {
                case HudEvent.UPDATE:
                {
                    HudEvent hudMsg = (HudEvent)evt;
                    if (hudMsg.Lifes != null)
                    {
                        this.lifesValueLabel.Text = hudMsg.Lifes.ToString();
                    }
                    if (hudMsg.Score != null)
                    {
                        this.scoreValueLabel.Text = hudMsg.Score.ToString();
                    }
                    break;
                }
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            // Workaround: Otherweise arrow keys won't be treated as input keys but as special keys and won't work
            // when the HUD is displayed.
            if (keyData == Keys.Up
                || keyData == Keys.Right
                || keyData == Keys.Down
                || keyData == Keys.Left)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

    }
}
