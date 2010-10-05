using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Entities
{
    class Actor : Entity
    {
        public Actor(BaseGame game, string name, string sprite, float posX, float posY, int width, int height, 
                     float speed)
            : base(game, name)
        {
            AddBehaviour(new SpatialBehaviour(this, posX, posY, width, height, speed));
            AddBehaviour(new RenderBehaviour(this, sprite));
        }
    }
}
