using System;
using System.Windows.Forms;
using Game;
using Game.EventManagement;
using Game.EventManagement.Events;

namespace SpaceInvaders.Controls
{
	public partial class GameMainMenu : UserControl
	{
		private IEventManager eventManager;

		public GameMainMenu(IEventManager eventManager)
		{
			InitializeComponent();

			this.eventManager = eventManager;
		}

		private void NewGameButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Loading));
		}

		private void HighscoreButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Highscore));
		}

		private void CreditsButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Credits));
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Quit));
		}


		/**********************************************************************
		 * Mouse over Button Effect Handler 
		 **********************************************************************/

		private void NewGameButton_MouseEnter(object sender, EventArgs e)
		{
			this.NewGameButton.Image = Properties.Resources.newgameOver;
		}

		private void NewGameButton_MouseLeave(object sender, EventArgs e)
		{
			this.NewGameButton.Image = Properties.Resources.newgame;
		}

		private void HighscoreButton_MouseEnter(object sender, EventArgs e)
		{
			this.HighscoreButton.Image = Properties.Resources.highscoreOver;
		}

		private void HighscoreButton_MouseLeave(object sender, EventArgs e)
		{
			this.HighscoreButton.Image = Properties.Resources.highscore;
		}

		private void CreditsButton_MouseEnter(object sender, EventArgs e)
		{
			this.CreditsButton.Image = Properties.Resources.creditsOver;
		}

		private void CreditsButton_MouseLeave(object sender, EventArgs e)
		{
			this.CreditsButton.Image = Properties.Resources.credits;
		}

		private void QuitButton_MouseEnter(object sender, EventArgs e)
		{
			this.QuitButton.Image = Properties.Resources.exitOver;
		}

		private void QuitButton_MouseLeave(object sender, EventArgs e)
		{
			this.QuitButton.Image = Properties.Resources.exit;
		}
	}
}
