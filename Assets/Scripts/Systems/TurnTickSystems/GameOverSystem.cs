using UnityEngine;
using UnityEngine.UI;

public class GameOverSystem
{
	public void Tick()
	{
		if (GameState.Instance.StageWon || GameState.Instance.GameOver)
		{
			return;
		}

		var player = Object.FindObjectOfType<Player>();

		if (player == null)
		{
			GameState.Instance.GameOver = true;

			var textPanelObj = GameObject.Find("TextPanel").GetComponent<Image>();
			var gameOverText = GameObject.Find("DeathText").GetComponent<Text>();

			textPanelObj.enabled = true;
			gameOverText.enabled = true;
		}
	}
}