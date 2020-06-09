using Unity.Entities;
using UnityEngine;

[UpdateBefore(typeof(TurnSystem))]
public class InputSystem : ComponentSystem
{
	private GameplayState gameState;

	protected override void OnCreate()
	{
		this.gameState = Object.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		gameState.Command = Command.NONE;

		if (Input.GetKey(KeyCode.W))
		{
			gameState.Command = Command.MOVEUP;
		}

		if (Input.GetKey(KeyCode.A))
		{
			gameState.Command = Command.TURNLEFT;
		}

		if (Input.GetKey(KeyCode.S))
		{
			gameState.Command = Command.MOVEDOWN;
		}

		if (Input.GetKey(KeyCode.D))
		{
			gameState.Command = Command.TURNRIGHT;
		}

		if (Input.GetKey(KeyCode.E))
		{
			gameState.Command = Command.MOVERIGHT;
		}

		if (Input.GetKey(KeyCode.Q))
		{
			gameState.Command = Command.MOVELEFT;
		}

		if (Input.GetKey(KeyCode.Space))
		{
			gameState.Command = Command.FIRE;
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			gameState.Command = Command.PASSTURN;
		}
	}
}