using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.Utility;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game;

namespace SpaceInvaders.Controls
{
	public partial class HighscoreScreen : UserControl
	{
		private IEventManager eventManager;
		private List<Tuple<Label, Label, Label>> rows = new List<Tuple<Label, Label, Label>>();

		public HighscoreScreen(IEventManager eventManager)
		{
			this.eventManager = eventManager;

			InitializeComponent();

			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel1, dateLabel1, scoreLabel1));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel2, dateLabel2, scoreLabel2));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel3, dateLabel3, scoreLabel3));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel4, dateLabel4, scoreLabel4));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel5, dateLabel5, scoreLabel5));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel6, dateLabel6, scoreLabel6));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel7, dateLabel7, scoreLabel7));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel8, dateLabel8, scoreLabel8));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel9, dateLabel9, scoreLabel9));
			rows.Add(new Tuple<Label, Label, Label>(playerNameLabel10, dateLabel10, scoreLabel10));
		}

		private void playerNameLabel2_VisibleChanged(object sender, EventArgs e)
		{
			Highscore.Reload();

			for (int i = 0; i < Highscore.List.Count; i++)
			{
				var row = rows[i];
				var entry = Highscore.List.ElementAt(i);

				row.Item1.Text = entry.Name;
				row.Item2.Text = entry.Date.ToString("dd.MM.yyyy, HH:mm");
				row.Item3.Text = entry.Score.ToString();
			}
		}

		private void backButton_Click(object sender, EventArgs e)
		{
			eventManager.Queue(GameStateChangedEvent.To(GameState.Menu));
		}
	}
}
