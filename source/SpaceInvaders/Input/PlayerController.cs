using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Game.EventManagement;
using Game.EventManagement.Events;
using Game.Entities;

namespace SpaceInvaders.Input
{
    class PlayerController : IKeyboardHandler
    {
        private Entity playerEntity;
        private IEventManager eventManager;

        public PlayerController(Entity playerEntity)
        {
            this.playerEntity = playerEntity;
            this.eventManager = playerEntity.EventManager;
        }

        #region IKeyboardHandler Members

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, playerEntity.ID, Direction.North));
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, playerEntity.ID, Direction.West));
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, playerEntity.ID, Direction.South));
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, playerEntity.ID, Direction.East));
                    break;
                case Keys.Space:
                    eventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.START_FIRING, playerEntity.ID));
                    break;
            }
        }

        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode) {
                case Keys.W:
                    goto case Keys.Up;
                case Keys.Up:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.STOP_MOVING, playerEntity.ID, Direction.North));
                    break;
                case Keys.A:
                    goto case Keys.Left;
                case Keys.Left:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.STOP_MOVING, playerEntity.ID, Direction.West));
                    break;
                case Keys.S:
                    goto case Keys.Down;
                case Keys.Down:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.STOP_MOVING, playerEntity.ID, Direction.South));
                    break;
                case Keys.D:
                    goto case Keys.Right;
                case Keys.Right:
                    eventManager.QueueEvent(new MoveEvent(MoveEvent.STOP_MOVING, playerEntity.ID, Direction.East));
                    break;
                case Keys.Space:
                    eventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.STOP_FIRING, playerEntity.ID));
                    break;
            }
        }

        #endregion
    }
}
