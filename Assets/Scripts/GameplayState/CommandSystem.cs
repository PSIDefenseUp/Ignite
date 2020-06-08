using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class CommandSystem : ComponentSystem
{
	private GameplayState gameState;

	protected override void OnCreate()
	{
		this.gameState = GameObject.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		var playerQuery = GetEntityQuery(typeof(PlayerComponent), typeof(ActorComponent));

		using(var players = playerQuery.ToEntityArray(Allocator.TempJob))
		{
			UnitAction action = UnitAction.NONE;

			foreach (var command in gameState.Commands)
			{
				switch(command)
				{
					case Command.MOVEUP:
						action = UnitAction.MOVEUP;
						break;
					case Command.MOVEDOWN:
						action = UnitAction.MOVEDOWN;
						break;
					case Command.MOVELEFT:
						action = UnitAction.MOVELEFT;
						break;
					case Command.MOVERIGHT:
						action = UnitAction.MOVERIGHT;
						break;
					case Command.TURNLEFT:
						action = UnitAction.TURNLEFT;
						break;
					case Command.TURNRIGHT:
						action = UnitAction.TURNRIGHT;
						break;
					case Command.FIRE:
						action = UnitAction.FIRE;
						break;
					case Command.PASSTURN:
						action = UnitAction.PASSTURN;
						break;
					default:
						break;
				}

				// action = command switch
				// {
				// 	Command.MOVEUP => UnitAction.MOVEUP,
				// 	Command.MOVEDOWN => UnitAction.MOVEDOWN,
				// 	Command.MOVELEFT => UnitAction.MOVELEFT,
				// 	Command.MOVERIGHT => UnitAction.MOVERIGHT,
				// 	Command.TURNLEFT => UnitAction.TURNLEFT,
				// 	Command.TURNRIGHT => UnitAction.TURNRIGHT,
				// 	_ => UnitAction.NONE,
				// };
			}

			foreach (var player in players)
			{
				var actorComponent = EntityManager.GetComponentData<ActorComponent>(player);
				EntityManager.SetComponentData<ActorComponent>(player, actorComponent.SetAction(action));
			}
		}
	}
}