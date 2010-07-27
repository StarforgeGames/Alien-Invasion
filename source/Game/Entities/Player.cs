using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Entities
{

    public class Player : IBehaviour
    {
        List<IBehaviour> behaviours;

        public Player()
        {
            behaviours = new List<IBehaviour>();
            behaviours.Add(new SpatialBehaviour());
            behaviours.Add(new RenderBehaviour());
        }

        #region IBehaviour Members

        public void OnUpdate(float deltaTime)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnUpdate(deltaTime);
            }
        }

        public void OnMessage(Message msg)
        {
            foreach (IBehaviour b in behaviours) {
                b.OnMessage(msg);
            }
        }

        #endregion
    }

}
