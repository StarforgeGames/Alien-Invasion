using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Entities;
using Game.Behaviors;
using Game.EventManagement.Events;
using Game.EventManagement;
using Game.Utility;
using System.Diagnostics;
//using Logging;

namespace Game
{
	public class CollisionManager : IEventListener
	{
		public GameLogic Game { get; private set; }
		public IEventManager EventManager { get; private set; }

		private List<Entity> collidables;


		public CollisionManager(GameLogic game)
		{
			this.Game = game;
			this.EventManager = game.EventManager;

			this.collidables = new List<Entity>();

			registerGameEventListeners();
		}

		private void registerGameEventListeners()
		{
			EventManager.AddListener(this, typeof(NewEntityEvent));
			EventManager.AddListener(this, typeof(DestroyEntityEvent));
		}

		public void DetectAndResolveCollisions()
		{
			foreach (Entity entity in collidables) {
				if (entity.IsDead) {
					continue;
				}

				foreach (Entity other in collidables) {
					if (entity.ID == other.ID 
						|| other.IsDead
						|| (entity.Type.StartsWith("pewpew") && other.Type.StartsWith("pewpew")))
					{
						continue;
					}

					var entityFaction = entity[CollisionBehavior.Key_Faction];
					var otherFaction = other[CollisionBehavior.Key_Faction];
					// No friendly fire... or collision
					if (entityFaction == otherFaction) {
						continue;
					}

					if (AreColliding(entity, other)) {
						Console.WriteLine("[" + this.GetType().Name + "] " + entity.Type + " collided with " 
							+ other.Type + "!");

						CollisionEvent collisionMsg = CollisionEvent.Collides(other.ID, entity.ID);
						EventManager.Queue(collisionMsg);

						int collisionDmg = entity[CollisionBehavior.Key_CollisionDamage];
						DamageEvent dmgMsg = DamageEvent.Receive(collisionDmg, entity.ID, other.ID);
						EventManager.Queue(dmgMsg);
					}
				}
			}
			
		}
		
		public void Reset()
		{
			collidables.Clear();
		}

		public bool AreColliding(Entity entity, Entity other)
		{
			Vector2D position = entity[SpatialBehavior.Key_Position];
			Vector2D dimensions = entity[SpatialBehavior.Key_Dimensions];

			Vector2D otherPosition = other[SpatialBehavior.Key_Position];
			Vector2D otherDimensions = other[SpatialBehavior.Key_Dimensions];

			return position.X <= otherPosition.X + otherDimensions.X
				&& position.Y <= otherPosition.Y + otherDimensions.Y
				&& position.X + dimensions.X >= otherPosition.X
				&& position.Y + dimensions.Y >= otherPosition.Y;
		}

		public void OnEvent(Event evt)
		{
			switch (evt.Type) {
				case NewEntityEvent.NEW_ENTITY: {
						NewEntityEvent newEntityEvent = (NewEntityEvent)evt;
						Entity entity = Game.World.Entities[newEntityEvent.EntityID];
						OnAttach(entity);
						break;
					}
				case DestroyEntityEvent.DESTROY_ENTITY: {
						DestroyEntityEvent destroyEntityEvent = (DestroyEntityEvent)evt;
						Entity entity = Game.World.Entities[destroyEntityEvent.EntityID];
						OnDetach(entity);
						break;
					}
			}
		}

		public void OnAttach(Entity entity)
		{
			if (entity.HasBehavior(typeof(CollisionBehavior)))
			{
				// Log.Error("Adding " + entity.ToString() + "to collidables.", this.GetType().Name);
				collidables.Add(entity);
			}
		}

		public void OnDetach(Entity entity)
		{
			if (collidables.Contains(entity)) {
				//Log.Error("Removing " + entity.ToString() + "from collidables.", this.GetType().Name);
				collidables.Remove(entity);
			}
		}
	}
}
