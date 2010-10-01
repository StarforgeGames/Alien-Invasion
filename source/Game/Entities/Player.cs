using Game.Behaviours;

namespace Game.Entities
{

    class Player : Entity
    {
        public Player() 
            : base()
        {
            AddBehaviour(new SpatialBehaviour(this, 1f, 50f, 50f));
            AddBehaviour(new RenderBehaviour(this, @"Gfx\player.png"));
        }
    }

}
