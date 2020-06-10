using UnityEngine;

public class GameManager : MonoBehaviour
{
	public void Update()
	{
		GameState.Instance.Tick();
	}
}