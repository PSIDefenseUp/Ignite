using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class ShamisenThinker : EnemyThinker
{
	private bool canDodge = true;
	private int actionNumber = 0;
	private readonly int2[] moveDirections = new int2[] { new int2(0, 1), new int2(1, 0), new int2(-1, 0) };
	private static Sprite bulletSprite;

	public void Start()
	{
		bulletSprite = Resources.Load<Sprite>("Final/Enemy_Bullet");
	}

	public override void Think()
	{
		var position = GetComponent<Position>();
		var actor = GetComponent<Actor>();
		IAction action = null;

		if (canDodge)
		{
			var dodgeDirection = GetDodgeDirection();

			if (dodgeDirection != null)
			{
				position.Rotation = dodgeDirection.Value;
				action = new WallAction(position.GetRelativePosition(new int2(0,1)), 1);
				canDodge = false;
			}
		}
		else
		{
			action = new PassTurnAction();
			canDodge = true;
		}

		if (action == null)
		{
			// standard behaviour
			switch(actionNumber)
			{
				case 0:
					TurnTowardsPlayer();
					action = Move(moveDirections);
					break;
				case 1:
					TurnTowardsPlayer();
					action = new WallAction(position.GetRelativePosition(new int2(0,1)), 1);
					break;
				default:
					break;
			}

			actionNumber = (actionNumber + 1) % 2;
		}

		actor.SetAction(action);
	}

	protected float? GetDodgeDirection()
	{
		var bullets = FindObjectsOfType<Bullet>().Where(bullet => bullet.Team != Team.ENEMY).Select(bullet => bullet.GetComponent<Position>());
		var position = GetComponent<Position>();

		foreach (var bullet in bullets)
		{
			//find bullet that is 2 moves away from hitting us and rotate towrds direction.
			if (bullet.GetRelativePosition(new int2(0, 2)).Equals(position.Value))
			{
				return position.GetClosestAbsoluteDirection(bullet.Value, 4);
			}
		}

		return null;
	}
}