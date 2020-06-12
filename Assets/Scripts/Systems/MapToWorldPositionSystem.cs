using UnityEngine;

public class MapToWorldPositionSystem
{
	public void Tick()
	{
		var mapPositions = Object.FindObjectsOfType<Position>();

		foreach (var position in mapPositions)
		{
			var isBackgroundTile = position.GetComponent<BackgroundTile>() != null;

			if (isBackgroundTile)
			{
				position.transform.position = new Vector3(position.Value.x, position.Value.y, GameState.Instance.Map.Dimensions.y);
				continue;
			}

			var move = position.GetComponent<SlipNSlide>();
			if (move != null)
			{
				var interpX = Mathf.Lerp(move.start.Value.x, move.end.Value.x, (GameState.Instance.SecondsBetweenTurns - GameState.Instance.SecondsUntilNextTurn) / GameState.Instance.SecondsBetweenTurns);
				var interpY = Mathf.Lerp(move.start.Value.y, move.end.Value.y, (GameState.Instance.SecondsBetweenTurns - GameState.Instance.SecondsUntilNextTurn) / GameState.Instance.SecondsBetweenTurns);

				if (move.end.Value.x == interpX && move.end.Value.y == interpY)
				{
					Object.Destroy(move);
				}

				position.transform.position = new Vector3(interpX, interpY, interpY);
			}
			else
			{
				position.transform.position = new Vector3(position.Value.x, position.Value.y, position.Value.y);
			}
		}
	}
}