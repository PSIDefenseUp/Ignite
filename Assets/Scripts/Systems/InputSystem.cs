using UnityEngine;

public class InputSystem
{
	public void Tick()
	{
		GameState.Instance.InputCommand = InputCommand.NONE;

		if (Input.GetKey(KeyCode.W))
		{
			GameState.Instance.InputCommand = InputCommand.MOVEUP;
		}

		if (Input.GetKey(KeyCode.A))
		{
			GameState.Instance.InputCommand = InputCommand.MOVELEFT;
		}

		if (Input.GetKey(KeyCode.S))
		{
			GameState.Instance.InputCommand = InputCommand.MOVEDOWN;
		}

		if (Input.GetKey(KeyCode.D))
		{
			GameState.Instance.InputCommand = InputCommand.MOVERIGHT;
		}

		if (Input.GetKey(KeyCode.Q))
		{
		}

		if (Input.GetKey(KeyCode.E))
		{
		}

		if (Input.GetKey(KeyCode.Space))
		{
			GameState.Instance.InputCommand = InputCommand.PASSTURN;
		}

		if (Input.GetMouseButton(0))
		{
			GameState.Instance.InputCommand = InputCommand.FIRE;
		}

		if (Input.GetMouseButton(1))
		{
			GameState.Instance.InputCommand = InputCommand.ALTFIRE;
		}
	}
}