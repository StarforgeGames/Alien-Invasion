using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;

namespace Game.Entities
{

    public class EntityFactory
    {
        private BaseGame game;

        public EntityFactory(BaseGame game)
        {
            this.game = game;
        }

        public Entity New(string id)
        {
            switch (id.ToLowerInvariant()) {
                case "player":
                    float x = game.WorldWidth / 2;
                    float y = game.WorldHeight - 100;
                    Entity player = new Actor(game, id, @"Gfx\player.png", x, y, 50f);
                    player.AddBehaviour(new CombatBehaviour(player, .75f));
                    player.AddBehaviour(new CollisionBehaviour(player));
                    player.AddBehaviour(new HealthBehaviour(player, 1));
                    return player;

                case "pewpew":
                    Entity pewpew = new Actor(game, id, @"Gfx\pewpew.png", 0f, 0f, 200f);
                    pewpew.AddBehaviour(new CollisionBehaviour(pewpew));
                    return pewpew;

                default:
                    return null;
            }
        }
    }

}
