using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class MapMoveSystem : ComponentSystem
{
	private GameplayState gameState;
	private EntityQuery mapPositionQuery;
	private NativeArray<MapPositionComponent> mapPositions;

	protected override void OnCreate()
	{
		this.mapPositionQuery = GetEntityQuery(typeof(MapPositionComponent));
		this.gameState = GameObject.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		this.mapPositions = mapPositionQuery.ToComponentDataArray<MapPositionComponent>(Allocator.TempJob);

		for (int i = 0; i < mapPositions.Length; i++)
		{
			var mapPosition = mapPositions[i];
			var desiredPosition = new MapPositionComponent(mapPosition.X + mapPosition.DeltaX, mapPosition.Y + mapPosition.DeltaY);

			if (gameState.Map.Contains(desiredPosition.X, desiredPosition.Y) && !IsOccupied(desiredPosition.X, desiredPosition.Y))
			{
				mapPositions[i] = desiredPosition;
			}
		}

		mapPositionQuery.CopyFromComponentDataArray(mapPositions);
		mapPositions.Dispose();
	}

	private bool IsOccupied(int x, int y)
	{
		foreach (var mapPosition in mapPositions)
		{
			if (mapPosition.X == x && mapPosition.Y == y)
			{
				return true;
			}
		};

		return false;
	}
}