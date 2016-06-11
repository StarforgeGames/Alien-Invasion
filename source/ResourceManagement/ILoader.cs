namespace ResourceManagement
{
    public interface ILoader
    {
        void Load(ResourceHandle resourceHandle);
        void Load(ResourceHandle resourceHandle, IEvent evt);
        void Reload(ResourceHandle resourceHandle);
        void Reload(ResourceHandle resourceHandle, IEvent evt);
        void Unload(ResourceHandle resourceHandle);
        void Unload(ResourceHandle resourceHandle, IEvent evt);
    }
}
