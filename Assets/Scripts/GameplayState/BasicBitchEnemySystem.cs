using Unity.Entities;

public class BasicBitchEnemySystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.WithAll(typeof(BasicBitchEnemyComponent), typeof(ActorComponent)).ForEach((ref MapPositionComponent mapPosition, ref ActorComponent actor) =>
		{
			actor = actor.SetAction(UnitAction.MOVERIGHT);
		});
	}
}