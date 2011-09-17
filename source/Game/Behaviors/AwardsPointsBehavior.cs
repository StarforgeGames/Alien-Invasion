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
            entity.AddAttribute(Key_PointsAwarded, new Attribute<int>(0));

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
                    if (this.entity.ID == msg.EntityID && msg.DestroyedByEntityID != null) {
                        Attribute<int> points = entity[Key_PointsAwarded] as Attribute<int>;
                        if (points.Value <= 0) {
                            break;
                        }

                        AwardPointsEvent awardPointsEvent = new AwardPointsEvent(AwardPointsEvent.AWARD_POINTS,
                            (int)msg.DestroyedByEntityID, points.Value, this.entity.ID);
                        eventManager.QueueEvent(awardPointsEvent);
                    }

                    break;
                }
            }
        }
    }
}
