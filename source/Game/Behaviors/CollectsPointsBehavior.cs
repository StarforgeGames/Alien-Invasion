using System;
using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{
    public class CollectsPointsBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_PointsCollected = "PointsCollected";

        public CollectsPointsBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_PointsCollected, 0);

            initializeHandledEventTypes();
        }

        protected override void initializeHandledEventTypes()
        {
            handledEventTypes.Add(typeof(AwardPointsEvent));
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        {
            switch (evt.Type) {
                case AwardPointsEvent.AWARD_POINTS: {
                    AwardPointsEvent msg = (AwardPointsEvent)evt;

                    int points = entity[Key_PointsCollected];
                    points += msg.Points;
                    entity[Key_PointsCollected] = points;

                    HudEvent hudEvent = HudEvent.UpdateScore(points);
                    eventManager.Queue(hudEvent);

                    Console.WriteLine("[" + this.GetType().Name + "] " + entity + " collected " + msg.Points
                        + " points for destroying " + msg.SourceEntityID + " (total points: " + points + " )");

                    break;
                }
            }
        }
    }
}
