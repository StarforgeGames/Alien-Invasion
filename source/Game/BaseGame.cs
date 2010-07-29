using System.Collections.Generic;
using Game.Entities;
using Device = SlimDX.Direct3D10.Device;

namespace Game
{

    public class BaseGame
    {
        public static Device D3DDevice { get; set; }

        private List<Entity> entities;

        public BaseGame(Device device)
        {
            D3DDevice = device;

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
