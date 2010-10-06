using System.Collections.Generic;
using Game.Entities;
using Game.Input;
using Game.Behaviours;

namespace Game
{

    public class BaseGame
    {
        public EntityFactory EntityFactory { get; private set; }
        public List<Entity> Entities { get; private set; }
        public CommandInterpreter PlayerInterpreter { get; private set; }

        public int WorldWidth { get; private set; }
        public int WorldHeight { get; private set; }

        public ProcessManager ProcessManager { get; private set; }

        public BaseGame(int worldWidth, int worldHeight)
        {
            this.WorldWidth = worldWidth;
            this.WorldHeight = worldHeight;

            EntityFactory = new EntityFactory(this);
            Entities = new List<Entity>();

            ProcessManager = new ProcessManager();

            Entity player = EntityFactory.New("player");
            Entities.Add(player);
            PlayerInterpreter = new CommandInterpreter(player);

            Entity alien_ray = EntityFactory.New("alien_ray");
            Entities.Add(alien_ray);
        }

        public Entity AddEntity(string id) 
        {
            Entity entity = EntityFactory.New(id);
            Entities.Add(entity);
            return entity;
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public void Update(float deltaTime)
        {
            List<Entity> tmp = new List<Entity>(Entities);

            foreach (Entity entity in tmp) {
                switch (entity.State) {
                    case EntityState.Active:
                        entity.Update(deltaTime);
                        break;
                    case EntityState.Dead:
                        Entities.Remove(entity);
                        break;
                }
            }

            ProcessManager.OnUpdate(deltaTime);
        }
    }

}
