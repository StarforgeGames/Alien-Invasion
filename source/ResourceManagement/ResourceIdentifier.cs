namespace ResourceManagement
{

    public class ResourceIdentifier
    {
        public string Name { get; private set; }
        public string Type { get; private set; }

        public ResourceIdentifier(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }

}
