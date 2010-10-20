using Game.EventManagement.Events;

namespace Game.EventManagement
{

    public interface IEventListener
    {
        void OnEvent(Event evt);
    }

}
