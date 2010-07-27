using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game;

namespace SpaceInvaders
{
    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application : Dx10Application
    {
        public GameLogic Game { get; set; }

        public Application()
        {
            this.GameTitle = "Space Invaders";
            Game = new GameLogic();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            int deltaMilliseconds = (int) deltaTime * 1000;
            Game.Update(deltaMilliseconds);
        }
    }
}
