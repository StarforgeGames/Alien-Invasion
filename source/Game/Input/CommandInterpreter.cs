using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.Messages;

namespace Game.Input
{
    public class CommandInterpreter
    {
        private Entity entity;

        public CommandInterpreter(Entity entity)
        {
            this.entity = entity;
        }

        public void StartMoving(Direction direction)
        {
            entity.SendMessage(new MoveMessage(direction));
        }

        public void StopMoving()
        {
            entity.SendMessage(new MoveMessage());
        }
    }
}
