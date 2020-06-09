using Unity.Entities;

[DisableAutoCreation]
public class TurnActionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref ActingThisTurn _, ref TurnAction turnAction, ref MapPositionComponent position, ref ActorComponent actor) => {
			position = position.RotateBy(turnAction.DesiredRotation);
			actor = actor.ConsumeAction(0);
			EntityManager.RemoveComponent<TurnAction>(entity);
		});
	}
}