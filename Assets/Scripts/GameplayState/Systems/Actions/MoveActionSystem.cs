using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class MoveActionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref ActingThisTurn _, ref MoveAction moveAction, ref MapPositionComponent position, ref ActorComponent actor) => {
			position = position.SetMoveRelative(moveAction.DeltaX, moveAction.DeltaY);
			actor = actor.ConsumeAction(1);
			EntityManager.RemoveComponent<MoveAction>(entity);
		});
	}
}