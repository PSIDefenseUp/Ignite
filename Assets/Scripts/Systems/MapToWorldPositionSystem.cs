using UnityEngine;

public class MapToWorldPositionSystem
{
	public void Tick()
	{
		var mapPositions = Object.FindObjectsOfType<Position>();

		foreach (var position in mapPositions)
		{
			var isBackgroundTile = position.GetComponent<BackgroundTile>() != null;
			position.transform.position = new Vector3(position.Value.x, position.Value.y, isBackgroundTile ? GameState.Instance.Map.Dimensions.y : position.Value.y);
		}
	}
}