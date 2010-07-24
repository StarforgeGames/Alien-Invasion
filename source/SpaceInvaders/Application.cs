using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    /// <summary>
    /// Concrete class for the game.
    /// </summary>
    class Application : Dx10Application
    {
        public Application()
        {
            this.GameTitle = "Space Invaders";
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            int deltaMilliseconds = (int) deltaTime * 1000;
        }
    }
}
