using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(TurnSystem))]
public class MapMoveSystem : ComponentSystem
{
	private GameplayState gameState;
	private EntityQuery mapPositionQuery;
	private EntityQuery solidQuery;
	private NativeArray<MapPositionComponent> mapPositions;
	private NativeArray<MapPositionComponent> solidPositions;

	protected override void OnCreate()
	{
		this.mapPositionQuery = GetEntityQuery(typeof(MapPositionComponent));
		this.solidQuery = GetEntityQuery(typeof(MapPositionComponent), typeof(SolidComponent));
		this.gameState = GameObject.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		this.mapPositions = mapPositionQuery.ToComponentDataArray<MapPositionComponent>(Allocator.TempJob);
		this.solidPositions = solidQuery.ToComponentDataArray<MapPositionComponent>(Allocator.TempJob);

		for (int i = 0; i < mapPositions.Length; i++)
		{
			var mapPosition = mapPositions[i];
			var desiredPosition = mapPosition.Move();

			if (gameState.Map.Contains(desiredPosition.X, desiredPosition.Y) && !IsOccupied(desiredPosition.X, desiredPosition.Y))
			{
				mapPositions[i] = desiredPosition;
			}
		}

		mapPositionQuery.CopyFromComponentDataArray(mapPositions);
		mapPositions.Dispose();
		solidPositions.Dispose();
	}

	private bool IsOccupied(int x, int y)
	{
		foreach (var solidPosition in solidPositions)
		{
			if (solidPosition.X == x && solidPosition.Y == y)
			{
				return true;
			}
		};

		return false;
	}
}