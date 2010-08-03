namespace Game.Behaviours
{

    public interface IBehaviour
    {
        void OnUpdate(float deltaTime);
        void OnMessage(Message msg);
    }

}
