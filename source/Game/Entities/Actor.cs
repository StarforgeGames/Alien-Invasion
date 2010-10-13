using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;
using Game.EventManagement.Events;

namespace Game.Entities
{

    class Actor : Entity
    {
        public Actor(GameLogic game, string name)
            : base(game, name)
        {
            AddBehaviour(new SpatialBehaviour(this));
            AddBehaviour(new RenderBehaviour(this));
        }
    }

}
