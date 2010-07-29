using System;
using Game.Entities;

namespace Game.Behaviours
{

    class SpatialBehaviour : IBehaviour
    {
        private Entity entity;

        public SpatialBehaviour(Entity entity)
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
