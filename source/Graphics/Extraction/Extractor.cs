using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Graphics.ResourceManagement;
using Game.Entities;

namespace Graphics
{
    public class Extractor
    {
        private Game.GameLogic game;


        private RenderObjects frontObjects = new RenderObjects(),
            backObjects = new RenderObjects();

        private void swap()
        {
            RenderObjects temp = frontObjects;
            frontObjects = backObjects;
            backObjects = temp;
        }

        public Extractor(Game.GameLogic game)
        {
            this.game = game;
        }
        public bool ExtractNext
        {
            private get;
            set;
        }

        public void ExtractSingle(Entity entity)
        {
            
        }


        public void OnUpdate(float deltaTime)
        {
            if (ExtractNext)
            {
                frontObjects.Clear();
                foreach (var GameObject in game.Entities)
                {
                    if (GameObject.Value["renderable"] != null)
                    {
                        ExtractSingle(GameObject.Value);
                    }
                }
                swap();
            }
        }
    }
}
