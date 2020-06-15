using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class EnemyThinker : Thinker
{
	public override void Think()
	{

	}

	protected virtual int2? ShouldDodge(int2[] dodgeDirections)
	{
		var currentPosition = GetComponent<Position>();
		var bulletPositions = FindObjectsOfType<Bullet>().Where(bullet => bullet.Team != Team.ENEMY).Select(bullet => bullet.GetComponent<Position>());

		if (WillBeHitByBullet(currentPosition.Value))
		{
			// find somewhere to dodge to
			dodgeDirections = dodgeDirections.Shuffle();

			foreach(var dodgeDirection in dodgeDirections)
			{
				var dodgeSpace = currentPosition.GetAbsoluteOffset(dodgeDirection) + currentPosition.Value;
				if (!bulletPositions.Any(bulletPosition => bulletPosition.Value.Equals(dodgeSpace)))
				{
					var moveAction = new MoveRelativeAction(dodgeDirection);
					if (moveAction.CanPerform(gameObject))
					{
						return dodgeDirection;
					}
				}
			}
		}

		return null;
	}

	protected bool WillBeHitByBullet(int2 position)
	{
		var bulletPositions = FindObjectsOfType<Bullet>().Where(bullet => bullet.Team != Team.ENEMY).Select(bullet => bullet.GetComponent<Position>());

		foreach (var bullet in bulletPositions)
		{
			var nextBulletPosition = bullet.Value + bullet.GetAbsoluteOffset(new int2(0, 1));
			if (nextBulletPosition.Equals(position))
			{
				return true;
			}
		}

		return false;
	}

	protected IAction Move(int2[] moveDirections)
	{
		// move in an open direction (where there is not a solid)
		// choose between valid options at random
		var position = GetComponent<Position>();
		var solids = FindObjectsOfType<Solid>();
		var bullets = FindObjectsOfType<Bullet>().Where(bullet => bullet.Team != Team.ENEMY);
		var randomlyOrderedDirections = moveDirections.Shuffle();

		foreach(var direction in randomlyOrderedDirections)
		{
			var moveAction = new MoveAction(direction.x, direction.y);
			if (moveAction.CanPerform(gameObject))
			{
				return moveAction;
			}
		}

		return null;
	}

	protected void TurnTowardsPlayer(Actor actor)
	{
		var position = actor.GetComponent<Position>();
		var player = FindObjectOfType<Player>();

		if (player != null)
		{
			var playerPosition = player.GetComponent<Position>();
			position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
		}
	}

	protected IAction Fire(Actor actor)
	{
		var position = actor.GetComponent<Position>();
		return new FireAction(position.GetAbsoluteOffset(new int2(0, 1)) + position.Value, position.Rotation, 1, Team.ENEMY);
	}
}