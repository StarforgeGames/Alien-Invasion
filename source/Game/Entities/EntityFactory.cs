using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Behaviors;
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
            Entity entity = new Entity(game, id);
            entity.Load(@"data\entities\" + id + ".xml");

            switch (id.ToLowerInvariant()) {
                case "player": {
                    Attribute<string> sprite = entity[RenderBehavior.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"data\sprites\player.png";

                    Attribute<float> speed = entity[SpatialBehavior.Key_Speed] as Attribute<float>;
                    speed.Value = 150f;
                    
                    Attribute<int> lifes = entity[HealthBehavior.Key_Lifes] as Attribute<int>;
                    lifes.Value = 3;

                    Attribute<float> firingSpeed = entity[CombatBehavior.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;

                    Attribute<float> timeSinceLastShot = entity[CombatBehavior.Key_TimeSinceLastShot] 
                        as Attribute<float>;
                    timeSinceLastShot.Value = firingSpeed;
                    break;
                }
            case "pewpew": {
                    Attribute<string> sprite = entity[RenderBehavior.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"data\sprites\pewpew.png";

                    Attribute<float> speed = entity[SpatialBehavior.Key_Speed] as Attribute<float>;
                    speed.Value = (300f);

                    Attribute<bool> isMoving = entity[SpatialBehavior.Key_IsMoving] as Attribute<bool>;
                    isMoving.Value = true;

                    Attribute<Vector2D> orientation = entity[SpatialBehavior.Key_Orientation] as Attribute<Vector2D>;
                    orientation.Value.X = 0;
                    orientation.Value.Y = -1;
                    break;
                }
                case "alien_ray": {
                    Attribute<string> sprite = entity[RenderBehavior.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"data\sprites\alien_ray.png";

                    Attribute<float> speed = entity[SpatialBehavior.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;
                    Attribute<int> lifes = entity[HealthBehavior.Key_Lifes] as Attribute<int>;
                    lifes.Value = 3;

                    Attribute<float> firingSpeed = entity[CombatBehavior.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;
                    break;
                }
                case "alien_pincher": {
                    Attribute<string> sprite = entity[RenderBehavior.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"data\sprites\alien_pincher.png";

                    Attribute<float> speed = entity[SpatialBehavior.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;

                    Attribute<float> firingSpeed = entity[CombatBehavior.Key_FiringSpeed] as Attribute<float>;
                    firingSpeed.Value = 0.75f;
                    break;
                }
                case "alien_hammerhead": {
                    Attribute<string> sprite = entity[RenderBehavior.Key_Sprite] as Attribute<string>;
                    sprite.Value = @"data\sprites\alien_hammerhead.png";

                    Attribute<float> speed = entity[SpatialBehavior.Key_Speed] as Attribute<float>;
                    speed.Value = 75f;

                    Attribute<float> firingSpeed = entity[CombatBehavior.Key_FiringSpeed] as Attribute<float>;
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
