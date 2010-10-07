using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.EventManagement.Events;

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
            entity.EventManager.QueueEvent(new MoveEvent(MoveEvent.START_MOVING, direction));
        }

        public void StopMoving(Direction direction)
        {
            entity.EventManager.QueueEvent(new MoveEvent(MoveEvent.STOP_MOVING, direction));
        }

        public void StartFiringWeapon()
        {
            entity.EventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.START_FIRING));
        }

        public void StopFiringWeapon()
        {
            entity.EventManager.QueueEvent(new FireWeaponEvent(FireWeaponEvent.STOP_FIRING));
        }
    }

}
