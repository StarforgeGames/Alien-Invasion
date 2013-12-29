using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.EventManagement.Events
{
    public class HudEvent : Event
    {
        // Event Message Types
        public const string UPDATE = "UPDATE_HUD";

        public int? Lifes { get; private set; }
        public int? Score { get; private set; }

        public HudEvent(string type, int? lifes, int? score)
            : base(type)
        {
            this.Lifes = lifes;
            this.Score = score;
        }

        public static HudEvent UpdateLifes(int lifes)
        {
            return new HudEvent(UPDATE, lifes, null);
        }

        public static HudEvent UpdateScore(int score)
        {
            return new HudEvent(UPDATE, null, score);
        }

        public override string ToString()
        {
            return base.ToString() + " [Lifes: " + Lifes + ", Score: " + Score + "]";
        }
    }
}
