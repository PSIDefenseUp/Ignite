using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FairyThinker : Thinker
{
	private int actionNumber = 0;
	private readonly int2[] moveDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		IAction action = null;

		switch(actionNumber)
		{
			case 0:
				action = Move(actor);
				break;
			case 1:
				TurnTowardsPlayer(actor);
				action = Fire(actor);
				break;
			default:
				break;
		}

		actor.SetAction(action);
		actionNumber = (actionNumber + 1) % 2;
	}

	private IAction Move(Actor actor)
	{
		// move in an open direction (where there is not a solid)
		// choose between valid options at random
		var position = actor.GetComponent<Position>();
		var solids = Object.FindObjectsOfType<Solid>();
		var randomlyOrderedDirections = moveDirections.Shuffle();

		foreach(var direction in randomlyOrderedDirections)
		{
			var positionContainsSolid = solids.Any(solid => {
				var solidPosition = solid.GetComponent<Position>();

				if (solidPosition.Value.Equals(position.Value + direction))
				{
					return true;
				}

				return false;
			});

			if (!positionContainsSolid)
			{
				return new MoveAction(direction.x, direction.y);
			}
		}

		return null;
	}

	private void TurnTowardsPlayer(Actor actor)
	{
		var position = actor.GetComponent<Position>();
		var player = FindObjectOfType<Player>();

		if (player != null)
		{
			var playerPosition = player.GetComponent<Position>();
			position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
		}
	}

	private IAction Fire(Actor actor)
	{
		var position = actor.GetComponent<Position>();
		return new FireAction(position.GetAbsoluteOffset(new int2(0, 1)) + position.Value, position.Rotation, 1);
	}
}