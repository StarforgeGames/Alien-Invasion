using Game;
using Graphics;

namespace SpaceInvaders
{

    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application : Dx10Application
    {
        public BaseGame Game { get; set; }
        public Renderer Renderer { get; set; }

        public Application()
        {
            this.GameTitle = "Space Invaders";
        }

        public override void Initialize()
        {
            base.Initialize();
            Game = new BaseGame();
            Renderer = new Renderer(Device);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            int deltaMilliseconds = (int) deltaTime * 1000;
            Game.Update(deltaMilliseconds);
        }

        protected override void OnRender(float deltaTime)
        {
            Renderer.Render(deltaTime);
        }
    }

}
