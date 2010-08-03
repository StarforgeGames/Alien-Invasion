using Game.Behaviours;

namespace Game.Entities
{

    class Player : Entity
    {
        public Player()
            : base()
        {
            behaviours.Add(new SpatialBehaviour(this));
            behaviours.Add(new RenderBehaviour(this, "texture.bmp"));
        }
    }

}
