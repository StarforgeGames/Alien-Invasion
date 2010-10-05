using System;
using System.Collections.ObjectModel;
using Game.Messages;

namespace Game.Behaviours
{

    public interface IBehaviour
    {
        ReadOnlyCollection<Type> SupportedMessages { get; }

        void OnUpdate(float deltaTime);
        void OnMessage(Message msg);
    }

}
