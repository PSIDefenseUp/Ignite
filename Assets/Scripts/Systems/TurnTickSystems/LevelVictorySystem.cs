using UnityEngine;
using UnityEngine.UI;

public class LevelVictorySystem
{
	public void Tick()
	{
		if (GameState.Instance.StageWon || GameState.Instance.GameOver)
		{
			return;
		}

		var player = Object.FindObjectOfType<Player>();
		var endPoints = Object.FindObjectsOfType<LevelEndPoint>();

		if (player != null && endPoints != null)
		{
			var playerPosition = player.GetComponent<Position>();

			foreach (var endPoint in endPoints)
			{
				var endPointPosition = endPoint.GetComponent<Position>();

				if (endPointPosition != null && playerPosition.Value.Equals(endPointPosition.Value))
				{
					GameState.Instance.StageWon = true;

					var textPanelObj = GameObject.Find("TextPanel").GetComponent<Image>();
					var victoryText = GameObject.Find("VictoryText").GetComponent<Text>();

					textPanelObj.enabled = true;
					victoryText.enabled = true;
				}
			}
		}
	}
}