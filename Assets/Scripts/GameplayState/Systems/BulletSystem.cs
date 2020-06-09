using System;
using Unity.Entities;

public class BulletSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.WithAll(typeof(BulletComponent), typeof(ActorComponent), typeof(MapPositionComponent)).ForEach((Entity entity, ref MapPositionComponent mapPosition, ref ActorComponent actor) =>
		{
			// move forward by default
			var moveAction = new MoveAction(0, -1);
			EntityManager.AddComponentData(entity, moveAction);

			// TODO: if we will hit a static object, bounce

			// int directionIndex = new Random().Next(2);
			// float[] turnDirections = { 1f, 2f, 3f };
			// var turnAction = new TurnAction(turnDirections[directionIndex]);
			// EntityManager.SetComponentData(entity, turnAction);
		});
	}
}