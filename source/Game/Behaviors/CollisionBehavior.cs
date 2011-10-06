using System;
using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;

namespace Game.Behaviors
{
    public class CollisionBehavior : AEntityBasedBehavior
    {
        // Attribute Keys
        public const string Key_IsPhysical = "IsPhysical";
        public const string Key_CollisionDamage = "CollisionDamage";
        public const string Key_Faction = "Faction";

        public CollisionBehavior(Entity entity)
            : base(entity)
        {
            entity.AddAttribute(Key_IsPhysical, new Attribute<bool>(true));
            entity.AddAttribute(Key_CollisionDamage, new Attribute<int>(1));
            entity.AddAttribute(Key_Faction, new Attribute<string>(string.Empty));

            initializeHandledEventTypes();
        }

        public override void OnUpdate(float deltaTime)
        { }

        public override void OnEvent(Event evt)
        { }
    }
}
