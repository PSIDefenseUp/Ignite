using UnityEngine;

public class LevelVictorySystem
{
	public void Tick()
	{
		var player = Object.FindObjectOfType<Player>();
		var endPoints = Object.FindObjectsOfType<LevelEndPoint>();

		if (player != null && endPoints != null)
		{
			var playerPosition = player.GetComponent<Position>();

			foreach (var endPoint in endPoints)
			{
				var endPointPosition = endPoint.GetComponent<Position>();

				if (playerPosition.Value.Equals(endPointPosition.Value))
				{
					// TODO: END THE LEVEL, CONGRATS BIG WINNER
				}
			}
		}
	}
}