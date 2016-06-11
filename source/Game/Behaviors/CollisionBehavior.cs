using Game.Entities;
using Game.EventManagement.Events;

namespace Game.Behaviors
{
    public class CollisionBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_CollisionDamage = "CollisionDamage";
        public const string Key_Faction = "Faction";

        public CollisionBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_CollisionDamage, 1);
            entity.AddAttribute(Key_Faction, string.Empty);

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        { }
    }
}
