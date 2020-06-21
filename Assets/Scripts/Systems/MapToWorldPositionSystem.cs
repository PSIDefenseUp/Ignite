using UnityEngine;

public class MapToWorldPositionSystem
{
	public void Tick()
	{
		var mapPositions = Object.FindObjectsOfType<Position>();

		foreach (var position in mapPositions)
		{
			var isDecal = position.GetComponent<Decal>() != null;
			if (isDecal)
			{
				position.transform.position = new Vector3(position.Value.x, position.Value.y, GameState.Instance.Map.Dimensions.y);
				continue;
			}

			var move = position.GetComponent<SlipNSlide>();
			if (move != null && move.HasNotFinished)
			{
				var t = (GameState.Instance.SecondsBetweenTurns - GameState.Instance.SecondsUntilNextTurn) / GameState.Instance.SecondsBetweenTurns;
				var interpX = Mathf.Lerp(move.startPosition.Value.x, position.Value.x, t);
				var interpY = Mathf.Lerp(move.startPosition.Value.y, position.Value.y, t);

				if (position.Value.x == interpX && position.Value.y == interpY)
				{
					move.Finish();
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