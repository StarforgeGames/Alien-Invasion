using Game;

namespace SpaceInvaders
{
    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application : Dx10Application
    {
        public BaseGame Game { get; set; }

        public Application()
        {
            this.GameTitle = "Space Invaders";
        }

        public override void Initialize()
        {
            base.Initialize();
            Game = new BaseGame(Device);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            int deltaMilliseconds = (int) deltaTime * 1000;
            Game.Update(deltaMilliseconds);
        }
    }
}
