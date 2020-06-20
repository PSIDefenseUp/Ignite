using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerThinker : Thinker
{
	private static Sprite bulletSprite;

	public void Start()
	{
		bulletSprite = Resources.Load<Sprite>("Dev/Sprites/PlayerBullet");
	}

	public override void Think()
	{
		var command = GameState.Instance.InputCommand;

		var players = Object.FindObjectsOfType<Player>();
		var playerActors = players.Select(player => player.GetComponent<Actor>()).Where(actor => actor != null);

		foreach (var actor in playerActors)
		{
			var playerPosition = actor.gameObject.GetComponent<Position>();
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
					action = new TurnMoveAction(0, 1);
					break;
				case InputCommand.MOVEDOWN:
					action = new TurnMoveAction(0, -1);
					break;
				case InputCommand.MOVELEFT:
					action = new TurnMoveAction(-1, 0);
					break;
				case InputCommand.MOVERIGHT:
					action = new TurnMoveAction(1, 0);
					break;
				case InputCommand.TURNLEFT:
					action = new TurnAction(90);
					break;
				case InputCommand.TURNRIGHT:
					action = new TurnAction(-90);
					break;
				case InputCommand.FIRE:
					var bulletData = new BulletData(playerPosition.GetRelativePosition(new int2(0, 1)), playerPosition.Rotation, 1, Team.PLAYER, bulletSprite);
					
					action = new FireAction(bulletData);
					break;
				case InputCommand.ALTFIRE:
					action = new WallAction(playerPosition.Value + playerPosition.GetAbsoluteOffset(new int2(0, 1)), 1);
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