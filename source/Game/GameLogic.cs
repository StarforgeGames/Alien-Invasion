using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.Behaviours;

namespace Game
{

    public class GameLogic
    {
        List<IBehaviour> entities;

        public GameLogic()
        {
            entities = new List<IBehaviour>();
            entities.Add(new Player());
        }

        public void Update(float deltaTime)
        {
            foreach (IBehaviour entity in entities) {
                entity.OnUpdate(deltaTime);
            }
        }
    }

}
