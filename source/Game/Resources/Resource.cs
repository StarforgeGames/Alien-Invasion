using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Game.Resources
{

    public class Resource
    {
        private int id;
        public int ID
        {
            get { return id; }
        }

        private string name;
        public string Name
        {
            get { return name; }
        }

        private long size;
        public long Size
        {
            get {
                if (size == 0) {
                    Load();
                }
                return size; 
            }
        }

        private byte[] buffer;
        public byte[] Buffer
        {
            get {
                if (buffer == null) {
                    Load();
                }
                return buffer; 
            }
        }

        public Resource(int id, string name)
        {
            this.id = id;
            this.name = name;
            this.size = 0;
            this.buffer = null;
        }

        public void Load()
        {
            try {
                FileStream file = new FileStream(name, FileMode.Open);

                size = file.Length;
                buffer = new byte[size];
                
                try {
                    int check = file.Read(buffer, 0, (int)size);
	            }
	            catch (Exception) {
		
	            }
            }
            catch (Exception) {

            }
        }
    }

}
