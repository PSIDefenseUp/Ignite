using System;
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
			switch(gameState.Command)
			{
				case Command.MOVEUP:
					var moveUpAction = new MoveAction(0, -1);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, moveUpAction);
					}
					break;
				case Command.MOVEDOWN:
					var moveDownAction = new MoveAction(0, 1);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, moveDownAction);
					}
					break;
				case Command.MOVELEFT:
					var moveLeftAction = new MoveAction(-1, 0);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, moveLeftAction);
					}
					break;
				case Command.MOVERIGHT:
					var moveRightAction = new MoveAction(1, 0);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, moveRightAction);
					}
					break;
				case Command.TURNLEFT:
					var turnLeftAction = new TurnAction(Math.PI / 2);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, turnLeftAction);
					}
					break;
				case Command.TURNRIGHT:
					var turnRightAction = new TurnAction(-Math.PI / 2);
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, turnRightAction);
					}
					break;
				case Command.FIRE:
					foreach (var player in players)
					{
						var playerPosition = EntityManager.GetComponentData<MapPositionComponent>(player);
						var fireAction = new FireAction(playerPosition.Rotation);
						EntityManager.AddComponentData(player, fireAction);
					}
					break;
				case Command.PASSTURN:
					var passAction = new PassTurnAction();
					foreach (var player in players)
					{
						EntityManager.AddComponentData(player, passAction);
					}
					break;
				default:
					Debug.Log("NO ACTION");
					break;
			}
		}
	}
}