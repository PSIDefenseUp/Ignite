using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class EnemyThinker : Thinker
{
	private bool canDodge = true;
	private int actionNumber = 0;
	private readonly int2[] moveDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };
	private readonly int2[] dodgeDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		IAction action = null;

		// should we be using dodge behaviour?
		if (canDodge)
		{
			// TODO: decide whether or not we should dodge
			var dodgeDelta = ShouldDodge(dodgeDirections);

			if (dodgeDelta != null)
			{
				// TODO: DODGE
				action = new MoveAction(dodgeDelta.Value);
				canDodge = false;
			}
		}

		if (action == null)
		{
			// standard behaviour
			switch(actionNumber)
			{
				case 0:
					action = Move(actor, moveDirections);
					break;
				case 1:
					TurnTowardsPlayer(actor);
					action = Fire(actor);
					break;
				default:
					break;
			}

			actionNumber = (actionNumber + 1) % 2;
		}

		actor.SetAction(action);
	}

	protected virtual int2? ShouldDodge(int2[] dodgeDirections)
	{
		// TODO: determine if we should dodge this turn, and where to dodge to
		// get positions of all player bullets
		// if any will hit our location without a move, we probably want to dodge
		// check positions to (relative) left and right
		// if either one is both open and safe to move to (no bullet right there right now), dodge there
		// if not, don't dodge and continue with normal behaviour
		var currentPosition = GetComponent<Position>();
		var bulletPositions = Object.FindObjectsOfType<Bullet>().Select(bullet => bullet.GetComponent<Position>());

		foreach (var bullet in bulletPositions)
		{
			var nextBulletPosition = bullet.Value + bullet.GetAbsoluteOffset(new int2(0, 1));
			if (nextBulletPosition.Equals(currentPosition.Value))
			{
				// find somewhere to dodge to
				dodgeDirections = dodgeDirections.Shuffle();

				foreach(var dodgeDirection in dodgeDirections)
				{
					if (!bulletPositions.Any(bulletPosition => bulletPosition.Value.Equals(dodgeDirection)))
					{
						return dodgeDirection;
					}
				}
			}
		}

		return null;
	}

	private IAction Move(Actor actor, int2[] moveDirections)
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