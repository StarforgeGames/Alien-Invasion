using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.EventManagement;
using System.Windows.Forms;
using Game.EventManagement.Events;
using Game;
using Game.EventManagement.Debug;

namespace SpaceInvaders.Input
{
    class GameController : IKeyboardHandler
    {
        private IEventManager eventManager;

        public GameController(IEventManager eventManager)
        {
            this.eventManager = eventManager;
        }

        #region IKeyboardHandler Members

        public void OnKeyDown(object sender, KeyEventArgs e)
        { }       

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            switch (e.KeyCode) {                
                case Keys.P:
                    goto case Keys.Pause;
                case Keys.Pause:
                    eventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.Paused));
                    break;
                case Keys.Escape:
                    eventManager.QueueEvent(new GameStateChangedEvent(GameStateChangedEvent.GAME_STATE_CHANGED,
                        GameState.Menu));
                    break;

                // Debug Keys
                case Keys.F9:
                    eventManager.Trigger(new DebugEvent(DebugEvent.DECREASE_SPEED));
                    break;
                case Keys.F10:
                    eventManager.Trigger(new DebugEvent(DebugEvent.SINGLE_STEP));
                    break;
                case Keys.F11:
                    eventManager.Trigger(new DebugEvent(DebugEvent.INCREASE_SPEED));
                    break;
                case Keys.F12:
                    eventManager.Trigger(new DebugEvent(DebugEvent.RESET_SPEED));
                    break;

                default:
                    e.Handled = false;
                    break;
            }
        }

        #endregion
    }
}
