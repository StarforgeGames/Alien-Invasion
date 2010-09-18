using System;
using Game.Entities;
using Game.Resources;

namespace Game.Behaviours
{

    class RenderBehaviour : IBehaviour
    {
        private Entity entity;

        public RenderBehaviour(Entity entity, string sprite)
        {
            this.entity = entity;
            Resource res = BaseGame.Resources.GetResource(sprite);
            entity.Attributes.Add("Sprite", sprite);
            entity.Attributes.Add("SpriteResourceID", res.ID.ToString());
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
