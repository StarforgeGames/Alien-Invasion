﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.Input;
using Game.Messages;

namespace SpaceInvaders.Input
{
    class KeyboardHandler : IKeyboardHandler
    {
        private CommandInterpreter interpreter;

        public KeyboardHandler(CommandInterpreter interpreter)
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
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Left;
                case Keys.Up:
                    goto case Keys.Left;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    interpreter.StopMoving();
                    break;
                case Keys.S:
                    goto case Keys.Left;
                case Keys.Down:
                    goto case Keys.Left;
                case Keys.D:
                    goto case Keys.Left;
                case Keys.Right:
                    goto case Keys.Left;
            }
        }

        #endregion
    }
}
