using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;
using System;

namespace Game.Behaviors
{
	public class AiMysteryShipBehavior : AEntityBasedBehavior
	{
		private float phasePosition;
		private float oldXPosition;
		
		public AiMysteryShipBehavior(Entity entity)
			: base(entity)
		{
			initializeHandledEventTypes();
		}

		protected override void initializeHandledEventTypes()
		{
			handledEventTypes.Add(typeof(DestroyEntityEvent));
		}

		public override void OnUpdate(float deltaTime)
		{
			Vector2D position = entity[SpatialBehavior.Key_Position];
			if (oldXPosition == 0) {
				oldXPosition = position.X;
			}

			float diff = Math.Abs(oldXPosition - position.X);
			phasePosition = (phasePosition + diff) % 360;
			oldXPosition = position.X;

			position.Y += (float)(75 * Math.Sin(phasePosition * Math.PI / 180)) * deltaTime;
			entity[SpatialBehavior.Key_Position] = position;

			if (position.X < -100 || position.X > game.World.Width + 100) {
				var destroyEntity = DestroyEntityEvent.Destroy(entity.ID);
				eventManager.Queue(destroyEntity);
			}
		}

		public override void OnEvent(Event evt)
		{
			switch (evt.Type)
			{
				case DestroyEntityEvent.DESTROY_ENTITY:
					DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;

					if (entity.ID != destroyEntityEvent.EntityID)
					{
						break;
					}

					AudioEvent audioEvent = AudioEvent.StopSound(entity.ID, "mystery_ship_loop", "aliens");
					eventManager.Queue(audioEvent);
					break;
			}
		}
	}
}
