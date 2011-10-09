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
    public class SoundLoader : ASingleThreadedLoader<byte[], bool>, IFileLoader
    {
        private IAudioPlayer player;

        public SoundLoader(IAudioPlayer player) : base(player.Queue)
        {
            this.player = player;
        }

        ~SoundLoader()
        {
        }

        public ResourceNameConverter Converter
        {
            get { return converter; }
        }

        private readonly ResourceNameConverter converter =
            new ResourceNameConverter(@"data\audio\", @".wav");

        public override string Type
        {
            get { return "audio"; }
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
            return new SoundResource(player.CreateSoundFrom(res));
        }
    }
}
