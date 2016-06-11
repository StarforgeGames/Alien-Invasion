using System;

namespace Game.Utility
{
	public class HighscoreEntry
	{
		public int Score { get; private set; }
		public string Name { get; private set; }
		public DateTime Date { get; private set; }

		public HighscoreEntry(int score, string name, DateTime date)
		{
			this.Score = score;
			this.Name = name;
			this.Date = date;
		}
	}
}
