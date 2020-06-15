using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FairyThinker : EnemyThinker
{
	private bool canDodge = true;
	private int actionNumber = 0;
	private readonly int2[] moveDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };
	private readonly int2[] dodgeDirections = new int2[] { new int2(1, 0), new int2(-1, 0) };

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
				action = new MoveRelativeAction(dodgeDelta.Value);
				canDodge = false;
			}
		}

		if (action == null)
		{
			// standard behaviour
			switch(actionNumber)
			{
				case 0:
					action = Move(moveDirections);
					break;
				case 1:
					TurnTowardsPlayer(actor);
					action = Fire(actor);
					break;
				default:
					break;
			}

			canDodge = true;
			actionNumber = (actionNumber + 1) % 2;
		}

		actor.SetAction(action);
	}
}