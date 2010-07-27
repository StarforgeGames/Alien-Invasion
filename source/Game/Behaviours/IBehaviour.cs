using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Behaviours
{

    interface IBehaviour
    {
        void OnUpdate(float deltaTime);
        void OnMessage(Message msg);
    }

}
