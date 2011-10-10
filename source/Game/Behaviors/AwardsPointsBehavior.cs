using System;
using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{
    public class AwardsPointsBehavior : AEntityBasedBehavior
    {
        public const string Key_PointsAwarded = "PointsAwarded";

        public AwardsPointsBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_PointsAwarded, 0);

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(DestroyEntityEvent));
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case DestroyEntityEvent.DESTROY_ENTITY: {
                    DestroyEntityEvent msg = (DestroyEntityEvent)evt;
                    if (this.entity.ID == msg.EntityID && msg.DestroyedByEntityID > 0) 
                    {
                        int points = entity[Key_PointsAwarded];
                        if (points <= 0) 
                        {
                            break;
                        }

                        Entity otherEntity;
                        game.World.Entities.TryGetValue(msg.DestroyedByEntityID, out otherEntity);
                        if (otherEntity == null || otherEntity.IsDead)
                        {
                            break;
                        }

                        AwardPointsEvent awardPointsEvent = AwardPointsEvent.Award(msg.DestroyedByEntityID, points,
                            this.entity.ID);
                        eventManager.Queue(awardPointsEvent);
                    }

                    break;
                }
            }
        }
    }
}
