using Unity.Entities;

public class BulletSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.WithAll(typeof(BulletComponent), typeof(ActorComponent)).ForEach((ref MapPositionComponent mapPosition, ref ActorComponent actor) =>
		{
			actor = actor.SetAction(UnitAction.MOVEUP);
		});
	}
}