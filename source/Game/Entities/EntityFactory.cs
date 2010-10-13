﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviours;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Entities
{

    public class EntityFactory
    {
        private GameLogic game;

        public EntityFactory(GameLogic game)
        {
            this.game = game;
        }

        public Entity New(string id, Dictionary<string, object> attributes = null)
        {
            Entity entity = null;

            switch (id.ToLowerInvariant()) {
                case "player": {
                    entity = new Actor(game, id);
                    entity.AddBehaviour(new CombatBehaviour(entity));
                    entity.AddBehaviour(new CollisionBehaviour(entity));
                    entity.AddBehaviour(new HealthBehaviour(entity));

                    Attribute<string> sprite = entity[RenderBehaviour.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"Gfx\player.png";

                    Attribute<float> speed = entity[SpatialBehaviour.Key_Speed] as Attribute<float>;
                    speed.Value = 150f;
                    
                    Attribute<int> lifes = entity[HealthBehaviour.Key_Lifes] as Attribute<int>;
                    lifes.Value = 3;

                    Attribute<float> firingSpeed = entity[CombatBehaviour.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;

                    Attribute<float> timeSinceLastShot = entity[CombatBehaviour.Key_TimeSinceLastShot] 
                        as Attribute<float>;
                    timeSinceLastShot.Value = firingSpeed;
                    break;
                }
                case "pewpew": {
                    entity = new Actor(game, id);
                    entity.AddBehaviour(new ProjectileBehaviour(entity));
                    entity.AddBehaviour(new CollisionBehaviour(entity));

                    Attribute<string> sprite = entity[RenderBehaviour.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"Gfx\pewpew.png";

                    Attribute<float> speed = entity[SpatialBehaviour.Key_Speed] as Attribute<float>;
                    speed.Value = (300f);

                    Attribute<bool> isMoving = entity[SpatialBehaviour.Key_IsMoving] as Attribute<bool>;
                    isMoving.Value = true;

                    Attribute<Vector2D> orientation = entity[SpatialBehaviour.Key_Orientation] as Attribute<Vector2D>;
                    orientation.Value.X = 0;
                    orientation.Value.Y = -1;
                    break;
                }
                case "alien_ray": {
                    entity = new Actor(game, id);
                    entity.AddBehaviour(new CombatBehaviour(entity));
                    entity.AddBehaviour(new CollisionBehaviour(entity));
                    entity.AddBehaviour(new HealthBehaviour(entity));

                    Attribute<string> sprite = entity[RenderBehaviour.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"Gfx\alien_ray.png";

                    Attribute<float> speed = entity[SpatialBehaviour.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;
                    Attribute<int> lifes = entity[HealthBehaviour.Key_Lifes] as Attribute<int>;
                    lifes.Value = 3;

                    Attribute<float> firingSpeed = entity[CombatBehaviour.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;
                    break;
                }
                case "alien_pincher": {
                    entity = new Actor(game, id);
                    entity.AddBehaviour(new CombatBehaviour(entity));
                    entity.AddBehaviour(new CollisionBehaviour(entity));
                    entity.AddBehaviour(new HealthBehaviour(entity));

                    Attribute<string> sprite = entity[RenderBehaviour.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"Gfx\alien_pincher.png";

                    Attribute<float> speed = entity[SpatialBehaviour.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;

                    Attribute<float> firingSpeed = entity[CombatBehaviour.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;
                    break;
                }
                case "alien_hammerhead": {
                    entity = new Actor(game, id);
                    entity.AddBehaviour(new CombatBehaviour(entity));
                    entity.AddBehaviour(new CollisionBehaviour(entity));
                    entity.AddBehaviour(new HealthBehaviour(entity));

                    Attribute<string> sprite = entity[RenderBehaviour.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"Gfx\alien_hammerhead.png";

                    Attribute<float> speed = entity[SpatialBehaviour.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;

                    Attribute<float> firingSpeed = entity[CombatBehaviour.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;
                    break;
                }
                default:
                    // Unknown entity! Report and return
                    Console.WriteLine("[" + this.GetType().Name + "] Unknown entity requested: " + id);
                    return null;
            }

            Console.WriteLine("[" + this.GetType().Name + "] Created entity " + entity);

            if (attributes != null) {
                foreach (KeyValuePair<string, object> pair in attributes) {
                    entity[pair.Key] = pair.Value;
                }
            }

            NewEntityEvent newEntityEvent = new NewEntityEvent(NewEntityEvent.NEW_ENTITY, entity.ID);
            game.EventManager.QueueEvent(newEntityEvent);

            return entity;
        }
    }

}
