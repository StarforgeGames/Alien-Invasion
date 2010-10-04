using System.Collections.Generic;
using Game.Entities;
using Game.Input;

namespace Game
{

    public class BaseGame
    {
        private EntityFactory entityFactory;
        public EntityFactory EntityFactory { get { return entityFactory; } }
        public List<Entity> Entities { get; set; }
        public CommandInterpreter PlayerInterpreter { get; set; }

        public int WorldWidth { get; set; }
        public int WorldHeight { get; set; }

        public ProcessManager ProcessManager { get; set; }

        public BaseGame(int worldWidth, int worldHeight)
        {
            this.WorldWidth = worldWidth;
            this.WorldHeight = worldHeight;

            entityFactory = new EntityFactory(this);
            Entities = new List<Entity>();

            ProcessManager = new ProcessManager();

            Entity player = entityFactory.New("player");
            Entities.Add(player);
            PlayerInterpreter = new CommandInterpreter(player);
        }

        public Entity AddEntity(string id) 
        {
            Entity entity = entityFactory.New(id);
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
