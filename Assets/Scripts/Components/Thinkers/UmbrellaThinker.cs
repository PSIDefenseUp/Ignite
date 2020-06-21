using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class UmbrellaThinker : EnemyThinker
{
	private const int BLOCK_DURATION = 3;
	private int blockDurationRemaining = 0;

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var reflector = GetComponent<BulletReflector>();
		var playerPosition = FindObjectOfType<Player>()?.GetComponent<Position>();

		// pass turn while blocking
		if (blockDurationRemaining > 0)
		{
			actor.SetAction(new PassTurnAction());
			return;
		}

		// if not blocking, we shouldn't be reflecting
		if (reflector != null)
		{
			Destroy(reflector);
		}

		// check if we should block
		var bulletPositions = FindObjectsOfType<Bullet>()
			.Where(bullet => bullet.Team != Team.ENEMY)
			.Select(bullet => bullet.GetComponent<Position>());

		var threatBullets = bulletPositions
			.Where(bulletPosition => bulletPosition.GetRelativePosition(new int2(0, 1)).Equals(position.Value));

		if (threatBullets.Any())
		{
			// we are blocking now
			blockDurationRemaining = BLOCK_DURATION;
			reflector.enabled = false;

			// prioritize any bullet that's already in front of us
			var facingBullet = threatBullets.FirstOrDefault(bulletPosition =>
				bulletPosition.Equals(position.GetRelativePosition(new int2(0, 1)))
				|| bulletPosition.Equals(position.GetRelativePosition(new int2(1, 1)))
				|| bulletPosition.Equals(position.GetRelativePosition(new int2(-1, 1)))
			);

			if (facingBullet == null)
			{
				// if not already blocking in our current direction, turn to block a bullet
				var bulletToBlock = threatBullets.First();
				position.Rotation = position.GetClosestAbsoluteDirection(bulletToBlock.Value, 4);
			}
		}

		// if not dodging, move
		position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);

		if (position.GetRelativePosition(new int2(0, 1)).Equals(playerPosition.Value))
		{
			actor.SetAction(new DirectAttack(playerPosition.Value, Team.ENEMY, 1));
		}
		else if (position.Value.x == playerPosition.Value.x || position.Value.y == playerPosition.Value.y)
		{
			actor.SetAction(new MoveRelativeAction(0, 1));
		}
		else
		{
			// move left or right to get aligned with player
			var xDistance = playerPosition.Value.x - position.Value.x;
			var yDistance = playerPosition.Value.y - position.Value.y;

			if (Mathf.Abs(xDistance) <= Mathf.Abs(yDistance))
			{
				actor.SetAction(new MoveAction(1 * (int)Mathf.Sign(xDistance), 0));
			}
			else
			{
				actor.SetAction(new MoveAction(0, 1 * (int)Mathf.Sign(yDistance)));
			}
		}

		actor.SetAction(new PassTurnAction());
	}
}