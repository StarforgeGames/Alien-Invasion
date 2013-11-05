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
using Game;

namespace SpaceInvaders.Controls
{
	public partial class DefeatScreen : UserControl
	{
		private IEventManager eventManager;

		public DefeatScreen(IEventManager eventManager)
		{
			this.eventManager = eventManager;

			InitializeComponent();
		}

		private void continueButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Highscore));
		}
	}
}
