using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Entities
{

    class Entity
    {
        protected List<IBehaviour> behaviours;

        public Entity()
        {
            behaviours = new List<IBehaviour>();
        }

        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public void RemoveBehaviour(IBehaviour behaviour)
        {
            behaviours.Remove(behaviour);
        }

        public void SendMessage(Message msg)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnMessage(msg);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }
    }

}
