using System;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class TurnSystem : ComponentSystem
{
	private GameplayState gameState;

	private readonly float waitTime = .4f;
	private MapMoveSystem moveSystem;
	private CommandSystem commandSystem;

	protected override void OnCreate()
	{
		this.gameState = UnityEngine.Object.FindObjectOfType<GameplayState>();
		this.commandSystem = World.GetOrCreateSystem<CommandSystem>();
		this.moveSystem = World.GetOrCreateSystem<MapMoveSystem>();
	}

	protected override void OnUpdate()
	{
		if (gameState.CurrentWaitTime <= 0)
		{
			commandSystem.Update();

			// if all players have an action ready and have action points, or all players have already gone this round, take turn
			var playerQuery = GetEntityQuery(typeof(PlayerComponent), typeof(ActorComponent));
			var actorQuery = GetEntityQuery(typeof(ActorComponent));
			using (var players = playerQuery.ToEntityArray(Allocator.TempJob))
			{
				var allPlayersReady = true;
				var allPlayersHaveGone = true;
				foreach (var playerEntity in players)
				{
					var player = EntityManager.GetComponentData<ActorComponent>(playerEntity);

					if (player.Action == UnitAction.NONE)
					{
						allPlayersReady = false;
					}

					if (player.RemainingActions > 0)
					{
						allPlayersHaveGone = false;
					}

					if (!allPlayersReady && !allPlayersHaveGone)
					{
						break;
					}
				}

				if (allPlayersReady && !allPlayersHaveGone)
				{
					// let the player(s) go
					foreach (var playerEntity in players)
					{
						var actor = EntityManager.GetComponentData<ActorComponent>(playerEntity);

						ProcessUnitAction(playerEntity, actor);
					}

					moveSystem.Update(); // TODO: FIX THIS SHIT
					gameState.CurrentWaitTime = waitTime;
				}
				else if (allPlayersHaveGone)
				{
					// let everyone else go
					using (var actorEntities = actorQuery.ToEntityArray(Allocator.TempJob))
					{
						foreach (var entity in actorEntities)
						{
							var actor = EntityManager.GetComponentData<ActorComponent>(entity);

							ProcessUnitAction(entity, actor);
							EntityManager.SetComponentData(entity, actor.Refresh());
						}
					}

					moveSystem.Update(); // TODO: FIX THIS SHIT
					gameState.CurrentWaitTime = waitTime;
				}
			}
		}
		else
		{
			// decrease wait time
			gameState.CurrentWaitTime -= Time.DeltaTime;
		}
	}

	private void ProcessUnitAction(Entity entity, ActorComponent actor)
	{
		if (actor.RemainingActions > 0)
		{
			// TODO: move this shit anywhere but here what the fuck
			// this is also really awful in general and I hate it in so many ways, I blame unity
			var mapPosition = EntityManager.GetComponentData<MapPositionComponent>(entity);
			switch(actor.Action)
			{
				case UnitAction.MOVEUP:
					EntityManager.SetComponentData(entity, new MapPositionComponent(mapPosition, 0, -1));
					break;
				case UnitAction.MOVEDOWN:
					EntityManager.SetComponentData(entity, new MapPositionComponent(mapPosition, 0, 1));
					break;
				case UnitAction.MOVELEFT:
					EntityManager.SetComponentData(entity, new MapPositionComponent(mapPosition, -1, 0));
					break;
				case UnitAction.MOVERIGHT:
					EntityManager.SetComponentData(entity, new MapPositionComponent(mapPosition, 1, 0));
					break;
				case UnitAction.TURNLEFT:
					EntityManager.SetComponentData(entity, mapPosition.RotateBy(-Math.PI / 2));
					break;
				case UnitAction.TURNRIGHT:
					EntityManager.SetComponentData(entity, mapPosition.RotateBy(Math.PI / 2));
					break;
				case UnitAction.FIRE:
					break;
				default:
					break;
			}

			EntityManager.SetComponentData(entity, actor.ConsumeAction());
		}
	}
}