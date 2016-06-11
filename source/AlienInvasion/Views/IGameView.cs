using Game.Entities;
using Game.EventManagement;

namespace AlienInvasion.Views
{
    public enum GameViewType
    {
        PlayerView,
        AIView,
        RemotePlayerView
    }

    interface IGameView : IEventListener
    {
        GameViewType Type { get; }
        int ID { get; }

        void OnUpdate(float deltaTime);
        void OnAttach(Entity entity);
    }

}
