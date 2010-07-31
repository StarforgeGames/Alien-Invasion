using System.Collections.Generic;
using Game.Entities;

namespace Game
{

    public class BaseGame
    {
        private List<Entity> entities;

        public BaseGame()
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
