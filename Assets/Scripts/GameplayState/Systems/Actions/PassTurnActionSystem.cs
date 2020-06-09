using Unity.Entities;

[DisableAutoCreation]
public class PassTurnActionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref ActingThisTurn _, ref PassTurnAction __, ref ActorComponent actor) => {
			actor = actor.ConsumeAction(actor.RemainingActions);
			EntityManager.RemoveComponent<PassTurnAction>(entity);
		});
	}
}