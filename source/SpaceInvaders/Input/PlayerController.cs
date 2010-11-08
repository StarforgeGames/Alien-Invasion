using System.Windows.Forms;
using Game;
using Game.Entities;
using Game.EventManagement;
using Game.EventManagement.Events;
using System.Collections.Generic;
using System;
using Game.Utility;
using Game.EventManagement.Debug;

namespace SpaceInvaders.Input
{
    class PlayerController : IKeyboardHandler
    {
        private Entity playerEntity;
        private IEventManager eventManager;

        private Dictionary<Keys, bool> isKeyDown = new Dictionary<Keys, bool>();

        public PlayerController(Entity playerEntity)
        {
            this.playerEntity = playerEntity;
            this.eventManager = playerEntity.EventManager;

            foreach (Keys key in Enum.GetValues(typeof(Keys))) {
                if (!isKeyDown.ContainsKey(key)) {
                    isKeyDown.Add(key, false);
                }
            }
        }

        #region IKeyboardHandler Members

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            isKeyDown[e.KeyCode] = true;
            e.Handled = true;

            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    determineDirectionAndIssueMove(Direction.North, true);
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    determineDirectionAndIssueMove(Direction.East, true);
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    determineDirectionAndIssueMove(Direction.South, true);
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    determineDirectionAndIssueMove(Direction.West, true);
                    break;
                case Keys.Space:
                    eventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.START_FIRING, playerEntity.ID));
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void determineDirectionAndIssueMove(Direction newDirection, bool startMovingIntoDirection)
        {
            Vector2D direction = new Vector2D(0, 0);

            if (isKeyDown[Keys.Up] || isKeyDown[Keys.W]) {
                setMoveDirection(ref direction, Direction.North);
            }

            if (isKeyDown[Keys.Right] || isKeyDown[Keys.D]) {
                setMoveDirection(ref direction, Direction.East);
            }

            if (isKeyDown[Keys.Down] || isKeyDown[Keys.S]) {
                setMoveDirection(ref direction, Direction.South);
            }

            if (isKeyDown[Keys.Left] || isKeyDown[Keys.A]) {
                setMoveDirection(ref direction, Direction.West);
            }

            // Last Key press takes precedence over all other Key states to let the controls feel responsive
            if (startMovingIntoDirection) {
                setMoveDirection(ref direction, newDirection);
            }

            string type = startMovingIntoDirection ? MoveEvent.START_MOVING : MoveEvent.STOP_MOVING;
            eventManager.QueueEvent(new MoveEvent(type, playerEntity.ID, direction));
        }

        private void setMoveDirection(ref Vector2D vec, Direction direction)
        {
            switch (direction) {
                case Direction.North:
                    vec.Y = 1;
                    break;
                case Direction.East:
                    vec.X = 1;
                    break;
                case Direction.South:
                    vec.Y = -1;
                    break;
                case Direction.West:
                    vec.X = -1;
                    break;
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            isKeyDown[e.KeyCode] = false;
            e.Handled = true;

            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    determineDirectionAndIssueMove(Direction.North, false);
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    determineDirectionAndIssueMove(Direction.East, false);
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    determineDirectionAndIssueMove(Direction.South, false);
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    determineDirectionAndIssueMove(Direction.West, false);
                    break;
                case Keys.Space:
                    eventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.STOP_FIRING, playerEntity.ID));
                    break;
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
