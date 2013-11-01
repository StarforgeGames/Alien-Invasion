using Game.Entities;
using Game.EventManagement.Events;
using Game.Utility;
using System;

namespace Game.Behaviors
{
	public class AiInvaderBehavior : AEntityBasedBehavior
	{

		private float elapsedTime;
		private double timeToNextIdleAnimation;

		private static Random random = new Random();

		public AiInvaderBehavior(Entity entity)
			: base(entity)
		{ 
			initializeHandledEventTypes();

			timeToNextIdleAnimation = getTimeToNextIdleAnimation();
		}

		private double getTimeToNextIdleAnimation() 
		{
			return 10d + (random.NextDouble() * 30d);	// TODO: Tweak formula?
		}

		protected override void initializeHandledEventTypes()
		{ }

		public override void OnUpdate(float deltaTime)
		{
			Vector2D atBoundary = entity[SpatialBehavior.Key_AtBoundary];

			if (atBoundary.X < 0 || atBoundary.X > 0 || atBoundary.Y < 0 || atBoundary.Y > 0) {
				eventManager.Queue(AiUpdateMovementEvent.AtBorder(entity.ID, atBoundary));
			}

			elapsedTime += deltaTime;
			if (elapsedTime >= timeToNextIdleAnimation)
			{
				elapsedTime = 0.0f;
				timeToNextIdleAnimation = getTimeToNextIdleAnimation();

				eventManager.Queue(AnimationEvent.Play(entity.ID));
			}
		}

		public override void OnEvent(Event evt)
		{ }
	}
}
