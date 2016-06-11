namespace ResourceManagement.Loaders
{
    public interface IResourceLoader : ILoader
    {
        string Type
        {
            get;
        }

        ResourceHandle Default
        {
            get;
        }
    }
}
