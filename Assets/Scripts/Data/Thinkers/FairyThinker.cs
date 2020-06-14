using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FairyThinker : Thinker
{
	private int actionNumber = 0;
	private readonly int2[] moveDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };

	public override void Think()
	{
		IAction action = null;

		switch(actionNumber)
		{
			case 0:
				action = Move();
				break;
			case 1:
				TurnTowardsPlayer();
				action = Fire();
				break;
			default:
				break;
		}

		GetComponent<Actor>().SetAction(action);
		actionNumber = (actionNumber + 1) % 2;
	}

	private IAction Move()
	{
		// move in an open direction
		// choose between valid options at random
		var randomlyOrderedDirections = moveDirections.Shuffle();

		foreach(var direction in randomlyOrderedDirections)
		{
			var moveAction = new MoveAction(direction);

			if (moveAction.CanPerform(gameObject))
			{
				return new MoveAction(direction.x, direction.y);
			}
		}

		return null;
	}

	private void TurnTowardsPlayer()
	{
		var position = GetComponent<Position>();
		var player = FindObjectOfType<Player>();

		if (player != null)
		{
			var playerPosition = player.GetComponent<Position>();
			position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
		}
	}

	private IAction Fire()
	{
		var position = GetComponent<Position>();
		return new FireAction(position.GetAbsoluteOffset(new int2(0, 1)) + position.Value, position.Rotation, 1);
	}
}