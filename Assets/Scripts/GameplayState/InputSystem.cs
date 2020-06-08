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
			gameState.Commands.Add(Command.MOVELEFT);
		}

		if (Input.GetKey(KeyCode.S))
		{
			gameState.Commands.Add(Command.MOVEDOWN);
		}

		if (Input.GetKey(KeyCode.D))
		{
			gameState.Commands.Add(Command.MOVERIGHT);
		}
	}
}