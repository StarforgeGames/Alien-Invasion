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
            entity.Attributes.Add("PositionX", "0");
            entity.Attributes.Add("PositionY", "0");
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
