using UnityEngine;

public class MapToWorldPositionSystem
{
	public void Tick()
	{
		var mapPositions = Object.FindObjectsOfType<Position>();

		foreach (var position in mapPositions)
		{
			position.transform.position = new Vector3(position.Value.x, position.Value.y, 0);
			position.transform.rotation = Quaternion.EulerAngles(0, 0, position.Rotation);
		}
	}
}