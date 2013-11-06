using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResourceManagement.Loaders;
using ResourceManagement.Resources;
using ResourceManagement;
using Audio.Resources;
using FMOD;
using System.IO;

namespace Audio.Loaders
{
	public class StreamLoader : ASingleThreadedLoader<byte[], bool>, IFileLoader
	{
		private IAudioPlayer player;

		public StreamLoader(IAudioPlayer player) : base(player.Queue)
		{
			this.player = player;
		}

		~StreamLoader()
		{
		}

		public ResourceNameConverter Converter
		{
			get { return converter; }
		}

		private readonly ResourceNameConverter converter = new ResourceNameConverter(@"data\audio\", @".ogg");

		public override string Type
		{
			get { return "stream"; }
		}

		protected override void doUnload(AResource resource)
		{
			((SoundResource)resource).Sound.release();
		}

		protected override byte[] ReadResourceWithName(string name, out bool data)
		{
			data = false;
			var fileName = converter.getFilenameFrom(name);
			return File.ReadAllBytes(fileName);
		}

		protected override AResource doLoad(byte[] res, bool data)
		{
			return new SoundResource(player.CreateStreamFrom(res));
		}
	}
}
