using System.Collections.Generic;
using Game.Entities;
using Game.Input;

namespace Game
{

    public class BaseGame
    {
        public List<Entity> Entities { get; set; }
        public CommandInterpreter PlayerInterpreter { get; set; }

        public BaseGame()
        {
            Entities = new List<Entity>();
            Entity player = new Player();
            Entities.Add(player);
            PlayerInterpreter = new CommandInterpreter(player);
        }

        public void Update(float deltaTime)
        {
            foreach (Entity entity in Entities) {
                entity.Update(deltaTime);
            }
        }
    }

}
