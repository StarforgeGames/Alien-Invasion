using System.Collections.Generic;
using Game.Entities;
using Game.Resources;

namespace Game
{

    public class BaseGame
    {
        public static ResourceCache Resources { get; set; }

        public List<Entity> Entities { get; set; }

        public BaseGame()
        {
            Resources = new ResourceCache();

            Entities = new List<Entity>();
            Entities.Add(new Player());

        }

        public void Update(float deltaTime)
        {
            foreach (Entity entity in Entities) {
                entity.Update(deltaTime);
            }
        }
    }

}
