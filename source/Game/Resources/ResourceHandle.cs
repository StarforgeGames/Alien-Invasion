using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Resources
{

    public struct ResourceHandle
    {
        private uint size;
        public uint Size {
            get { return size; } 
        }

        private byte[] buffer;
        public byte[] Buffer {
            get { return buffer; }
        }

        public ResourceHandle(ref byte[] buffer, uint size)
        {
            this.buffer = buffer;
            this.size = size;
        }

        public int Load(IResourceFile file)
        {
            return file.GetData(out buffer);
        }
    }

}
