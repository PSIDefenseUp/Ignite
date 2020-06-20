using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DeployedWeaselThinker : EnemyThinker
{
	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var player = FindObjectOfType<Player>();
		var playerPosition = player?.GetComponent<Position>();
		var move = actor.GetComponent<SlipNSlide>();
		move.Init(position.Value);

		TurnTowardsPlayer();

		// action to move one space forward
		var moveAction = new MoveRelativeAction(new int2(0, 1));
		bool blockedByPlayer = false;

		// if we can move the first space, do it
		if (moveAction.CanPerform(this.gameObject))
		{
			position.Value = position.GetRelativePosition(new int2(0, 1));

			// if we can move the second space, do it
			if (moveAction.CanPerform(this.gameObject))
			{
				position.Value = position.GetRelativePosition(new int2(0, 1));
			}
			else if (player != null && playerPosition.Value.Equals(position.GetRelativePosition(new int2(0, 1))))
			{
				blockedByPlayer = true;
			}
		}
		else if (player != null && playerPosition.Value.Equals(position.GetRelativePosition(new int2(0, 1))))
		{
			blockedByPlayer = true;
		}

		if (blockedByPlayer)
		{
			// attack player if they are in the way of any part of our move
			player.GetComponent<Health>().Value--;
		}

		actor.SetAction(new PassTurnAction());

		if (GetComponent<Health>() == null)
		{
			gameObject.AddComponent<Health>();
			gameObject.GetComponent<Health>().Value = 1;
		}
	}
}
