using Unity.Entities;
using UnityEngine;

public class BasicBitchEnemySystem : ComponentSystem
{
	private GameplayState gameState;

	protected override void OnCreate()
	{
		this.gameState = Object.FindObjectOfType<GameplayState>();
	}

	protected override void OnUpdate()
	{
		Entities.WithAll(typeof(BasicBitchEnemyComponent), typeof(ActorComponent)).ForEach((ref MapPositionComponent mapPosition, ref ActorComponent actor) =>
		{
			// var nextPos = mapPosition.GetRelativePosition(0, -1);

			// if (!gameState.Map.Contains(nextPos.x, nextPos.y))
			// {
			// 	// reverse direction or pick another one or something
			// }

			actor = actor.SetAction(UnitAction.MOVERIGHT);
		});
	}
}