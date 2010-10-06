using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Game.Messages;
using Game.Entities;
using Game.Utility;

namespace Game.Behaviours
{

    class ProjectileBehaviour : AEntityBasedBehaviour
    {
        // Attribute Keys
        public const string Key_ProjectileOwner = "ProjectileOwner";

        public ProjectileBehaviour(Entity entity, Entity owner)
            : base(entity)
        {
            entity.AddAttribute(Key_ProjectileOwner, new Attribute<Entity>(owner));
        }

        #region IBehaviour Members

        List<Type> supportedMessages = new List<Type>() {
            typeof(CollisionMessage)
        };
        public override ReadOnlyCollection<Type> SupportedMessages
        {
            get { return supportedMessages.AsReadOnly(); }
        }

        public override void OnUpdate(float deltaTime)
        {
            Attribute<Rectangle> bounds = entity[SpatialBehaviour.Key_Bounds] as Attribute<Rectangle>;

            if (bounds.Value.Top <= 0 || bounds.Value.Bottom >= entity.Game.WorldHeight) {
                entity.Kill();
                Console.WriteLine(entity.Name + " died in vain.");
            }
        }

        public override void OnMessage(Messages.Message msg)
        {
            switch (msg.Type) {
                case CollisionMessage.ACTOR_COLLIDES: {
                    entity.Kill();
                    Console.WriteLine(entity.Name + " died fulfilling its honorable duty.");
                }
                break;
            }
        }

        #endregion
    }

}
