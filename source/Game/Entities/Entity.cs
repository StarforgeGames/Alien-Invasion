using System.Collections.Generic;
using Game.Behaviours;

namespace Game.Entities
{

    public class Entity
    {
        public Dictionary<string, string> Attributes { get; set; }
        protected List<IBehaviour> behaviours;

        public Entity()
        {
            behaviours = new List<IBehaviour>();
            Attributes = new Dictionary<string, string>();
        }

        public void AddBehaviour(IBehaviour behaviour)
        {
            behaviours.Add(behaviour);
        }

        public bool RemoveBehaviour(IBehaviour behaviour)
        {
            return behaviours.Remove(behaviour);
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
