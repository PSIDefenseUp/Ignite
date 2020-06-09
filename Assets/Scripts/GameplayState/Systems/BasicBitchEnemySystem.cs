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
		Entities.WithAll(typeof(BasicBitchEnemyComponent), typeof(ActorComponent)).ForEach((Entity entity) =>
		{
			EntityManager.AddComponentData(entity, new MoveAction(1, 0));
		});
	}
}