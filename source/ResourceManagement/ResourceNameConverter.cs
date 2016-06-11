namespace ResourceManagement
{
    public class ResourceNameConverter
    {
        readonly string baseDirectory;
        readonly string extension;

        public ResourceNameConverter(string baseDirectory, string extension)
        {
            this.baseDirectory = normalize(baseDirectory);
            this.extension = normalize(extension);
        }

        private string normalize(string filename)
        {
            return filename.Replace("\\", "/");
        }

        public string getFilenameFrom(string resourceName)
        {
            return baseDirectory + resourceName + extension;
        }

        public bool isResourceFor(string filename)
        {
            string nFilename = normalize(filename);

            return nFilename.StartsWith(baseDirectory) && nFilename.EndsWith(extension);
        }

        public string getResourceNameFrom(string filename)
        {
            string nFilename = normalize(filename);

            return nFilename.Substring(baseDirectory.Length, nFilename.Length - (baseDirectory.Length + extension.Length));
        }
    }
}
