using System.Collections.Generic;
using Game.Entities;
using Game.Input;
using Game.Behaviours;
using Game.EventManagement;

namespace Game
{

    public class BaseGame
    {
        public SwappingEventManager EventManager { get; private set; }
        public ProcessManager ProcessManager { get; private set; }

        public List<Entity> Entities { get; private set; }
        public EntityFactory EntityFactory { get; private set; }
        public CommandInterpreter PlayerInterpreter { get; private set; }

        public int WorldWidth { get; private set; }
        public int WorldHeight { get; private set; }

        public BaseGame(int worldWidth, int worldHeight)
        {
            this.WorldWidth = worldWidth;
            this.WorldHeight = worldHeight;

            EventManager = new SwappingEventManager();
            ProcessManager = new ProcessManager();

            Entities = new List<Entity>();
            EntityFactory = new EntityFactory(this);

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
            EventManager.Tick();
            ProcessManager.OnUpdate(deltaTime);

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
        }
    }

}
