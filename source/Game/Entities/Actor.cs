using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Entities
{
    class Actor : Entity
    {
        public Actor(BaseGame game, string name, string sprite, float posX, float posY, float speed)
            : base(game, name)
        {
            AddBehaviour(new SpatialBehaviour(this, posX, posY, speed));
            AddBehaviour(new RenderBehaviour(this, sprite));
        }
    }
}
