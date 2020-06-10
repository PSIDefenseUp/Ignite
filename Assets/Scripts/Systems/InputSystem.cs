using UnityEngine;

public class InputSystem
{
	public void Tick()
	{
		GameState.Instance.InputCommand = InputCommand.NONE;

		if (Input.GetKey(KeyCode.W))
		{
			GameState.Instance.InputCommand = InputCommand.MOVEFORWARD;
		}

		if (Input.GetKey(KeyCode.A))
		{
			GameState.Instance.InputCommand = InputCommand.TURNLEFT;
		}

		if (Input.GetKey(KeyCode.S))
		{
			GameState.Instance.InputCommand = InputCommand.MOVEBACK;
		}

		if (Input.GetKey(KeyCode.D))
		{
			GameState.Instance.InputCommand = InputCommand.TURNRIGHT;
		}

		if (Input.GetKey(KeyCode.Q))
		{
			GameState.Instance.InputCommand = InputCommand.STRAFELEFT;
		}

		if (Input.GetKey(KeyCode.E))
		{
			GameState.Instance.InputCommand = InputCommand.STRAFERIGHT;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			GameState.Instance.InputCommand = InputCommand.FIRE;
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			GameState.Instance.InputCommand = InputCommand.PASSTURN;
		}
	}
}