using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class GameplayState : MonoBehaviour
{
	[SerializeField]
	private Mesh tileMesh;

	[SerializeField]
	private Material playerMaterial;

	[SerializeField]
	private Material enemyMaterial;

	[SerializeField]
	private Material wallMaterial;

	private AABB tileBounds;

	public Map Map { get; private set; }
	public Command Command { get; set; }
	public EntityManager EntityManager { get; private set; }
	public float TurnNumber { get; set; } // current turn number
	public float CurrentWaitTime { get; set; } // current time left to wait until next turn can be taken

	public void Start()
	{
		// turn info
		TurnNumber = 1;
		CurrentWaitTime = 0;

		// load level
		this.Map = new Map();
		this.Command = Command.NONE;

		EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		tileBounds = new AABB
		{
			Center = new float3(tileMesh.bounds.center),
			Extents = new float3(tileMesh.bounds.extents),
		};

		// Player Entity
		var playerMesh = new RenderMesh { mesh = tileMesh, material = playerMaterial };
		var player = EntityManager.CreateEntity();
		EntityManager.AddComponentData(player, new MapPositionComponent(1, 1));
		EntityManager.AddComponentData(player, new PlayerComponent());
		EntityManager.AddSharedComponentData(player, playerMesh);
		EntityManager.AddComponentData(player, new LocalToWorld());
		EntityManager.AddComponentData(player, new Translation());
		EntityManager.AddComponentData(player, new RenderBounds { Value = tileBounds });
		EntityManager.AddComponentData(player, new MainCameraTargetComponent());
		EntityManager.AddComponentData(player, new ActorComponent(1));
		EntityManager.AddComponentData(player, new Rotation());

		// Walls
		var wallMesh = new RenderMesh { mesh = tileMesh, material = wallMaterial };
		var wallArchetype = EntityManager.CreateArchetype(typeof(MapPositionComponent), typeof(LocalToWorld), typeof(Translation), typeof(RenderBounds), typeof(RenderMesh), typeof(SolidComponent));

		for (int y = 0; y < Map.Height; y++)
		{
			for (int x = 0; x < Map.Width; x++)
			{
				// cover map bounds with static collision
				if (x == 0 || y == 0 || x == Map.Width - 1 || y == Map.Height - 1)
				{
					var wallEntity = EntityManager.CreateEntity(wallArchetype);
					EntityManager.SetSharedComponentData(wallEntity, wallMesh);
					EntityManager.SetComponentData(wallEntity, new MapPositionComponent(x, y));
					EntityManager.SetComponentData(wallEntity, new RenderBounds { Value = tileBounds });
				}
			}
		}

		// Create dumb enemy
		var enemyMesh = new RenderMesh { mesh = tileMesh, material = enemyMaterial };
		var enemyArchetype = EntityManager.CreateArchetype(typeof(MapPositionComponent), typeof(LocalToWorld), typeof(Translation), typeof(RenderBounds), typeof(RenderMesh), typeof(BasicBitchEnemyComponent), typeof(ActorComponent));

		var enemy = EntityManager.CreateEntity(enemyArchetype);
		EntityManager.SetSharedComponentData(enemy, enemyMesh);
		EntityManager.SetComponentData(enemy, new MapPositionComponent(3, 4));
		EntityManager.SetComponentData(enemy, new RenderBounds{ Value = tileBounds });
		EntityManager.SetComponentData(enemy, new ActorComponent(1));

		var enemy2 = EntityManager.CreateEntity(enemyArchetype);
		EntityManager.SetSharedComponentData(enemy2, enemyMesh);
		EntityManager.SetComponentData(enemy2, new MapPositionComponent(2, 6));
		EntityManager.SetComponentData(enemy2, new RenderBounds{ Value = tileBounds });
		EntityManager.SetComponentData(enemy2, new ActorComponent(3));
	}
}