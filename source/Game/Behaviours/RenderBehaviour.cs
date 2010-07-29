using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;

namespace Game.Behaviours
{

    class RenderBehaviour : IBehaviour
    {
        private Entity entity;

        public RenderBehaviour(Entity entity)
        {
            this.entity = entity;
        }

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
