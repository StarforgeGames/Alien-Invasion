using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Entities;
using Game.Utility;

namespace Game.Behaviours
{
    class CollisionBehaviour : IBehaviour
    {
        private Entity entity;

        public CollisionBehaviour(Entity entity)
        {
            this.entity = entity;
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() { };
        public ReadOnlyCollection<Type> SupportedMessages
        {
            get
            {
                return supportedMessages.AsReadOnly();
            }
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity e in entity.Game.Entities) {

            }
        }

        public void OnMessage(Messages.Message msg)
        {

        }

        #endregion

        private bool isColliding(int posX, int posY)
        {
            Attribute<Vector2D> pos = entity[SpatialBehaviour.Key_Position] as Attribute<Vector2D>;

            return false;
        }
    }
}
