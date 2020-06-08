using Unity.Entities;
using UnityEngine;

public class InputSystem : ComponentSystem
{
	private GameplayState gameState;

	protected override void OnCreate()
	{
		this.gameState = GameObject.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		gameState.Commands.Clear();

		if (Input.GetKey(KeyCode.W))
		{
			gameState.Commands.Add(Command.MOVEUP);
		}

		if (Input.GetKey(KeyCode.A))
		{
			gameState.Commands.Add(Command.TURNLEFT);
		}

		if (Input.GetKey(KeyCode.S))
		{
			gameState.Commands.Add(Command.MOVEDOWN);
		}

		if (Input.GetKey(KeyCode.D))
		{
			gameState.Commands.Add(Command.TURNRIGHT);
		}

		if (Input.GetKey(KeyCode.E))
		{
			gameState.Commands.Add(Command.MOVERIGHT);
		}

		if (Input.GetKey(KeyCode.Q))
		{
			gameState.Commands.Add(Command.MOVELEFT);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			gameState.Commands.Add(Command.FIRE);
		}

		if (Input.GetKey(KeyCode.LeftShift))
		{
			gameState.Commands.Add(Command.PASSTURN);
		}
	}
}