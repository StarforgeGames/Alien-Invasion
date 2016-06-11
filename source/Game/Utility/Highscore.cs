using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Game.Utility
{
	public static class Highscore
	{
		private static readonly string XML_PATH = @"data\highscore.xml";

		/// <summary>
		/// Descending comparer so the highest score is returned first.
		/// </summary>
		class Comparer : IComparer<HighscoreEntry>
		{
			public int Compare(HighscoreEntry x, HighscoreEntry y)
			{
				int ascendingResult = Comparer<int>.Default.Compare(x.Score, y.Score);

				if (ascendingResult == 0)
				{
					ascendingResult = Comparer<DateTime>.Default.Compare(x.Date, y.Date);
				}

				// turn the result around
				return 0 - ascendingResult;
			}
		}

		private static SortedSet<HighscoreEntry> highscore;

		/// <summary>
		/// The Highscore in a sorted list, based on the score sorted in an descending order.
		/// </summary>
		public static SortedSet<HighscoreEntry> List 
		{ 
			get
			{
				if (highscore == null)
				{
					highscore = new SortedSet<HighscoreEntry>(new Highscore.Comparer());
					load();
				}

				return highscore;
			}
		}

		public static int LowestScore { 
			get
			{
				if (List.Count == 0)
				{
					return 0;
				}

				return List.Last().Score;
			}
		}

		public static void Add(String name, int score)
		{
			Reload();
			HighscoreEntry entry = new HighscoreEntry(score, name, DateTime.Now);
			highscore.Add(entry);

			while (highscore.Count > 10)
			{
				highscore.Remove(highscore.Last());
			}

			save(XML_PATH);
		}

		public static void Reload()
		{
			highscore.Clear();
			load();
		}

		public static bool IsNewHighscore(int score)
		{
			return (List.Count < 10 || score > LowestScore);
		}

		private static void load()
		{
			XDocument doc = getXDocument(XML_PATH);

			if (doc.Element("highscore") == null)
			{
				return;
			}

			var entries = doc.Element("highscore").Descendants("entry");
			foreach (var entry in entries)
			{
				int score = int.Parse(entry.Value);
				string name = entry.Attribute("name").Value;
				DateTime date = DateTime.ParseExact(entry.Attribute("date").Value, "yyyy.MM.dd HH:mm:ss", 
					CultureInfo.InvariantCulture);

				highscore.Add(new HighscoreEntry(score, name, date));
			}
		}

		private static XDocument getXDocument(string path)
		{
			if (File.Exists(path))
			{
				return XDocument.Load(path);
			}
			else
			{
				return new XDocument();
			}
		}

		private static void save(string saveToPath)
		{
			XDocument doc = new XDocument();
			XElement root = new XElement("highscore");
			doc.Add(root);

			foreach (var entry in highscore)
			{
				root.Add(new XElement("entry", 
					new XAttribute("name", entry.Name),
					new XAttribute("date", entry.Date.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture)), 
					entry.Score));
			}

			doc.Save(saveToPath);
		}
	}
}
