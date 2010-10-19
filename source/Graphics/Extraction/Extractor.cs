using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graphics
{
    public class Extractor
    {
        private Game.GameLogic game;

        private LinkedList<RenderObject> front, back;

        public Extractor(Game.GameLogic game)
        {
            this.game = game;
        }
        public bool Extract 
        {
            private get;
            set;
        }


        public void OnUpdate(float deltaTime)
        {
            if (Extract)
            {

            }
        }
    }
}
