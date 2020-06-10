using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerThinker : Thinker
{
	public override void Think()
	{
		var command = GameState.Instance.InputCommand;

		var players = Object.FindObjectsOfType<Player>();
		var playerActors = players.Select(player => player.GetComponent<Actor>()).Where(actor => actor != null);

		foreach (var actor in playerActors)
		{
			IAction action = null;

			switch(command)
			{
				case InputCommand.MOVEFORWARD:
					action = new MoveRelativeAction(0, 1);
					break;
				case InputCommand.MOVEBACK:
					action = new MoveRelativeAction(0, -1);
					break;
				case InputCommand.STRAFERIGHT:
					action = new MoveRelativeAction(-1, 0);
					break;
				case InputCommand.STRAFELEFT:
					action = new MoveRelativeAction(1, 0);
					break;
				case InputCommand.MOVEUP:
					action = new MoveAction(0, 1);
					break;
				case InputCommand.MOVEDOWN:
					action = new MoveAction(0, -1);
					break;
				case InputCommand.MOVELEFT:
					action = new MoveAction(-1, 0);
					break;
				case InputCommand.MOVERIGHT:
					action = new MoveAction(1, 0);
					break;
				case InputCommand.TURNLEFT:
					action = new TurnAction(90);
					break;
				case InputCommand.TURNRIGHT:
					action = new TurnAction(-90);
					break;
				case InputCommand.FIRE:
					var playerPosition = actor.gameObject.GetComponent<Position>();
					action = new FireAction(playerPosition.Value + playerPosition.GetAbsoluteOffset(new int2(0, 1)), playerPosition.Rotation, 1);

					break;
				case InputCommand.PASSTURN:
					action = new PassTurnAction();
					break;

				default:
					break;
			}

			actor.SetAction(action);
		}
	}
}