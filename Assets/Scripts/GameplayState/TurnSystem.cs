using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

public class TurnSystem : ComponentSystem
{
	private GameplayState gameState;

	private readonly float waitTime = .2f;
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
						}
					}

					moveSystem.Update(); // TODO: FIX THIS SHIT
					gameState.CurrentWaitTime = waitTime;
				}

				var allActorsHaveGone = true;
				using (var actorEntities = actorQuery.ToEntityArray(Allocator.TempJob))
				{
					foreach (var entity in actorEntities)
					{
						var actor = EntityManager.GetComponentData<ActorComponent>(entity);
						if (actor.RemainingActions > 1)
						{
							allActorsHaveGone = false;
							break;
						}
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
			var actionCost = 1;

			switch(actor.Action)
			{
				case UnitAction.MOVEUP:
					EntityManager.SetComponentData(entity, mapPosition.SetMoveRelative(0, -1));
					break;
				case UnitAction.MOVEDOWN:
					EntityManager.SetComponentData(entity, mapPosition.SetMoveRelative(0, 1));
					break;
				case UnitAction.MOVELEFT:
					EntityManager.SetComponentData(entity, mapPosition.SetMoveRelative(-1, 0));
					break;
				case UnitAction.MOVERIGHT:
					EntityManager.SetComponentData(entity, mapPosition.SetMoveRelative(1, 0));
					break;
				case UnitAction.TURNLEFT:
					EntityManager.SetComponentData(entity, mapPosition.RotateBy(Math.PI / 2));
					actionCost = 0;
					break;
				case UnitAction.TURNRIGHT:
					EntityManager.SetComponentData(entity, mapPosition.RotateBy(-Math.PI / 2));
					actionCost = 0;
					break;
				case UnitAction.PASSTURN:
					actionCost = actor.RemainingActions;
					break;
				case UnitAction.FIRE:
					var bullet = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation), typeof(LocalToWorld), typeof(RenderMesh), typeof(RenderBounds), typeof(MapPositionComponent), typeof(BulletComponent), typeof(ActorComponent));
					var spawnPos = mapPosition.GetRelativePosition(0, -1);

					// var bulletMesh = new Mesh();
					// Material bulletMaterial = null;
					// var bulletRenderMesh = new RenderMesh { mesh = bulletMesh, material = bulletMaterial };

					var bulletRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);

					var bulletRenderBounds = new AABB
					{
						Center = new float3(bulletRenderMesh.mesh.bounds.center),
						Extents = new float3(bulletRenderMesh.mesh.bounds.extents),
					};

					EntityManager.SetComponentData(bullet, new MapPositionComponent(mapPosition) { X = spawnPos.x, Y = spawnPos.y, DeltaX = 0, DeltaY = 0 });
					EntityManager.SetSharedComponentData(bullet, bulletRenderMesh);
					EntityManager.SetComponentData(bullet, new RenderBounds { Value = bulletRenderBounds });
					EntityManager.SetComponentData(bullet, new ActorComponent(1));
					break;
				default:
					break;
			}

			EntityManager.SetComponentData(entity, actor.ConsumeAction(actionCost));
		}
	}
}