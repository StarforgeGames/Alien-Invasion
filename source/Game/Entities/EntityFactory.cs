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
                case "player": {
                    float x = game.WorldWidth / 2f - (75f / 2f);
                    float y = game.WorldHeight - 100 - (75f / 2f);
                    Entity player = new Actor(game, id, @"Gfx\player.png", x, y, 75, 75, 50f);
                    player.AddBehaviour(new CombatBehaviour(player, .75f));
                    player.AddBehaviour(new CollisionBehaviour(player, 10));
                    player.AddBehaviour(new HealthBehaviour(player, 1));
                    return player;
                }
                case "pewpew": {
                    Entity pewpew = new Actor(game, id, @"Gfx\pewpew.png", 0f, 0f, 5, 15, 200f);
                    pewpew.AddBehaviour(new CollisionBehaviour(pewpew, 10));
                    pewpew.AddBehaviour(new ProjectileBehaviour(pewpew));
                    return pewpew;
                }
                case "alien_ray": {
                    float x = game.WorldWidth / 2 - (75f / 2f);
                    float y = 100;
                    Entity ray = new Actor(game, id, @"Gfx\player.png", x, y, 75, 75, 50f);
                    ray.AddBehaviour(new CombatBehaviour(ray, .75f));
                    ray.AddBehaviour(new CollisionBehaviour(ray, 10));
                    ray.AddBehaviour(new HealthBehaviour(ray, 1));
                    return ray;
                }
                default:
                    return null;
            }
        }
    }

}
