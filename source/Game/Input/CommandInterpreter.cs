using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.Messages;

namespace Game.Input
{
    /// <summary>
    /// Interprets commands from the View into game terms and actions, to decouple them.
    /// </summary>
    public class CommandInterpreter
    {
        private Entity entity;

        public CommandInterpreter(Entity entity)
        {
            this.entity = entity;
        }

        public void StartMoving(Direction direction)
        {
            entity.SendMessage(new MoveMessage(MoveMessage.START_MOVING, direction));
        }

        public void StopMoving(Direction direction)
        {
            entity.SendMessage(new MoveMessage(MoveMessage.STOP_MOVING, direction));
        }

        public void StartFiringWeapon()
        {
            entity.SendMessage(new FireWeaponMessage(FireWeaponMessage.START_FIRING));
        }

        public void StopFiringWeapon()
        {
            entity.SendMessage(new FireWeaponMessage(FireWeaponMessage.STOP_FIRING));
        }
    }

}
