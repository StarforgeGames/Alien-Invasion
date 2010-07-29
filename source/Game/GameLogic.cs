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
        List<Entity> entities;

        public GameLogic()
        {
            entities = new List<Entity>();
            entities.Add(new Player());
        }

        public void Update(float deltaTime)
        {
            foreach (Entity entity in entities) {
                entity.Update(deltaTime);
            }
        }
    }

}
