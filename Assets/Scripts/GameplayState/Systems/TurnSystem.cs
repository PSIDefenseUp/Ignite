using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class TurnSystem : ComponentSystem
{
	private GameplayState gameState;

	private readonly float waitTime = .2f;
	private MoveActionSystem moveSystem;
	private TurnActionSystem rotateSystem;
	private PassTurnActionSystem passTurnSystem;
	private FireActionSystem fireSystem;
	private CommandSystem commandSystem;

	protected override void OnCreate()
	{
		this.gameState = UnityEngine.Object.FindObjectOfType<GameplayState>();
		this.moveSystem = World.GetOrCreateSystem<MoveActionSystem>();
		this.commandSystem = World.GetOrCreateSystem<CommandSystem>();
		this.rotateSystem = World.GetOrCreateSystem<TurnActionSystem>();
		this.passTurnSystem = World.GetOrCreateSystem<PassTurnActionSystem>();
		this.fireSystem = World.GetOrCreateSystem<FireActionSystem>();
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
				var allPlayersHaveGone = true;
				foreach (var playerEntity in players)
				{
					var player = EntityManager.GetComponentData<ActorComponent>(playerEntity);

					if (player.RemainingActions > 0)
					{
						allPlayersHaveGone = false;
					}

					if (!allPlayersHaveGone)
					{
						break;
					}
				}

				if (!allPlayersHaveGone)
				{
					// let the player(s) go
					foreach (var playerEntity in players)
					{
						EntityManager.AddComponentData(playerEntity, new ActingThisTurn());
					}
				}
				else if (allPlayersHaveGone)
				{
					// let everyone else go
					using (var actorEntities = actorQuery.ToEntityArray(Allocator.TempJob))
					{
						foreach (var entity in actorEntities)
						{
							var actor = EntityManager.GetComponentData<ActorComponent>(entity);

							if (actor.RemainingActions > 0)
							{
								EntityManager.AddComponentData(entity, new ActingThisTurn());
							}
						}
					}
				}

				moveSystem.Update();
				rotateSystem.Update();
				passTurnSystem.Update();
				fireSystem.Update();

				var allActorsHaveGone = true;
				var didAnyoneAct = false;
				using (var actorEntities = actorQuery.ToEntityArray(Allocator.TempJob))
				{
					foreach (var entity in actorEntities)
					{
						EntityManager.RemoveComponent<ActingThisTurn>(entity);
						var actor = EntityManager.GetComponentData<ActorComponent>(entity);

						if (actor.RemainingActions > 1)
						{
							allActorsHaveGone = false;
						}

						if (actor.ActedThisTurn)
						{
							didAnyoneAct = true;
						}

						actor.ActedThisTurn = false;
						EntityManager.SetComponentData(entity, actor);
					}

					if (allActorsHaveGone)
					{
						foreach (var entity in actorEntities)
						{
							var actor = EntityManager.GetComponentData<ActorComponent>(entity);
							EntityManager.SetComponentData(entity, actor.Refresh());
						}
					}
				}

				if (didAnyoneAct)
				{
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
}