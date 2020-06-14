using Unity.Mathematics;
using UnityEngine;

public class Map
{
	public readonly int2 Dimensions;

	public Map()
	{
		this.Dimensions = new int2(20, 15);
	}

	public Map(int width, int height)
	{
		this.Dimensions = new int2(width, height);
	}

	public bool Contains(int x, int y)
	{
		return x >= 0 && y >= 0 && x < Dimensions.x && y < Dimensions.y;
	}

	public bool Contains(int2 position)
	{
		return position.x >= 0 && position.y >= 0 && position.x < Dimensions.x && position.y < Dimensions.y;
	}

	public static Map FromScene()
	{
		var positions = Object.FindObjectsOfType<Position>();

		// int? minX = null;
		// int? minY = null;
		int? maxX = null;
		int? maxY = null;

		// get minimum and maximum x, y
		foreach (var position in positions)
		{
			var positionMapX = Mathf.RoundToInt(position.transform.position.x);
			var positionMapY = Mathf.RoundToInt(position.transform.position.y);

			position.Value = new int2(positionMapX, positionMapY);

			// if (minX == null || position.Value.x < minX)
			// {
			// 	minX = position.Value.x;
			// }

			if (maxX == null || position.Value.x > maxX)
			{
				maxX = position.Value.x;
			}

			// if (minY == null || position.Value.y < minY)
			// {
			// 	minY = position.Value.y;
			// }

			if (maxY == null || position.Value.y > maxY)
			{
				maxY = position.Value.y;
			}
		}

		int width = maxX.Value + 1; //- minX.Value + 1;
		int height = maxY.Value + 1; //- minY.Value + 1;

		// foreach (var position in positions)
		// {
		// 	// shift those fucking positions, we start at 0
		// 	position.Value -= new int2(minX.Value, minY.Value);
		// }

		return new Map(width, height);
	}
}