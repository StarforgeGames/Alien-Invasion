using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{
    public class AwardPointsEvent : Event
    {
        // TODO: Add Factory Methods to simplify construction

        // Event Message Types
        public const string AWARD_POINTS = "ACTOR_AWARDED_POINTS";

        public int Points { get; set; }
        public int SourceEntityID { get; set; }

        public AwardPointsEvent(string type, int recipientID, int points, int sourceEntityID)
            : base(type, recipientID)
        {
            this.Points = points > 0 ? points : 0;  // Only positive amount can be awarded
            this.SourceEntityID = sourceEntityID;
        }

        public static AwardPointsEvent Award(int recipientID, int points, int sourceEntityID)
        {
            return new AwardPointsEvent(AWARD_POINTS, recipientID, points, sourceEntityID);
        }

        public override string ToString()
        {
            return base.ToString() + " [" + Points + " awarded to EntityID " + RecipientID + " by SourceEntityID "
                + SourceEntityID + "]";
        }
    }
}
