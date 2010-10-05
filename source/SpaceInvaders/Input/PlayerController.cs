using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.Input;
using Game.Messages;

namespace SpaceInvaders.Input
{
    class PlayerController : IKeyboardHandler
    {
        private CommandInterpreter interpreter;

        public PlayerController(CommandInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        #region IKeyboardHandler Members

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    interpreter.StartMoving(Direction.North);
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    interpreter.StartMoving(Direction.West);
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    interpreter.StartMoving(Direction.South);
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    interpreter.StartMoving(Direction.East);
                    break;
                case Keys.Space:
                    interpreter.StartFiringWeapon();
                    break;
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    interpreter.StopMoving(Direction.North);
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    interpreter.StopMoving(Direction.West);
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    interpreter.StopMoving(Direction.South);
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    interpreter.StopMoving(Direction.East);
                    break;
                case Keys.Space:
                    interpreter.StopFiringWeapon();
                    break;
            }
        }

        #endregion
    }
}
