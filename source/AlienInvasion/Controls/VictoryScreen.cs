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
using Game.Utility;

namespace AlienInvasion.Controls
{
	public partial class VictoryScreen : UserControl
	{
		private IEventManager eventManager;

		public int CurrentScore { get; set; }

		public VictoryScreen(IEventManager eventManager)
		{
			this.eventManager = eventManager;

			InitializeComponent();
		}

		private void continueButton_Click(object sender, EventArgs e)
		{
			if (Highscore.IsNewHighscore(CurrentScore))
			{
				if (String.IsNullOrEmpty(playerNameTextBox.Text))
				{
					// TODO: Inform user
					return;
				}

				Highscore.Add(playerNameTextBox.Text, CurrentScore);
			}

			eventManager.Queue(GameStateChangedEvent.To(GameState.Highscore));
		}

		private void VictoryScreen_VisibleChanged(object sender, EventArgs e)
		{
			if (Highscore.IsNewHighscore(CurrentScore))
			{
				newHighscoreGroupBox.Visible = true;
			}
			else
			{
				newHighscoreGroupBox.Visible = false;
			}
		}

		private void playerNameTextBox_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				continueButton_Click(sender, null);
				e.Handled = true;
				return;
			}
		}

		protected override bool IsInputKey(Keys keyData)
		{
			if (keyData == Keys.Left || keyData == Keys.Right)
			{
				return true;
			}

			return base.IsInputKey(keyData);
		}
	}
}
