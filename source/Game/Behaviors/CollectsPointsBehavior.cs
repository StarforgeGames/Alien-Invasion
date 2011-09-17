using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            entity.AddAttribute(Key_PointsCollected, new Attribute<int>(0));

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
                    Attribute<int> points = entity[Key_PointsCollected] as Attribute<int>;

                    points.Value += msg.Points;
                    Console.WriteLine("[" + this.GetType().Name + "] " + entity + " collected " + msg.Points
                        + " points for destroying " + msg.SourceEntityID + " (total points: " + points + " )");

                    break;
                }
            }
        }
    }
}
