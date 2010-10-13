using Game.Behaviours;

namespace Game.Entities
{

    class Player : Entity
    {
        public Player(BaseGame game)
            : base(game)
        {
            AddBehaviour(new SpatialBehaviour(this, 50f, 50f, 10f));
            AddBehaviour(new RenderBehaviour(this, @"Gfx\player.png"));
            AddBehaviour(new CombatBehaviour(this, 1f));
        }
    }

}
