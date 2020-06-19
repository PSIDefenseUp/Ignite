using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DeployedWeaselThinker : EnemyThinker
{
    // Start is called before the first frame update
	public override void Think()
	{
        var actor = GetComponent<Actor>();
		TurnTowardsPlayer();
        var moveAction = new MoveRelativeAction(new int2(0, 2));
        if (moveAction.CanPerform(this.gameObject))
        {
            actor.SetAction(moveAction);
        }
        else
        {
            var singleMoveAction = new MoveRelativeAction(new int2(0,1));
            if(singleMoveAction.CanPerform(this.gameObject))
            {
                actor.SetAction(moveAction);
            }
            else
            {
                actor.SetAction(new PassTurnAction());
            }
        }
	}
}
