using UnityEngine;

public class GameOverSystem
{
	public void Tick()
	{
		var player = Object.FindObjectOfType<Player>();

		if (player == null)
		{
			// TODO: GAME OVER
		}
	}
}