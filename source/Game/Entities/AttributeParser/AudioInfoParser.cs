using Game.Behaviors;
using System.Xml;

namespace Game.Entities.AttributeParser
{
	class AudioInfoParser : IAttributeParser
	{
		public string Type
		{
			get { return "AudioInfo"; }
		}

		public object Parse(XmlNode node)
		{
			AudioInfo info = new AudioInfo();

			foreach (XmlNode soundNode in node.ChildNodes)
			{
				AudioInfo.Sound sound = new AudioInfo.Sound();
				sound.EventName = soundNode.InnerText;
				sound.Project = soundNode.Attributes["project"].InnerText;

				info.Add(soundNode.Attributes["onGameEvent"].InnerText, sound);
			}

			return info;
		}
	}
}
