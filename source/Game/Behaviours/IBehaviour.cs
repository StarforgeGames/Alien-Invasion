namespace Game.Behaviours
{

    interface IBehaviour
    {
        void OnUpdate(float deltaTime);
        void OnMessage(Message msg);
    }

}
