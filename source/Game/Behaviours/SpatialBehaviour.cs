using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Behaviours
{

    class SpatialBehaviour : IBehaviour
    {
        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {

        }

        public void OnMessage(Message msg)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
