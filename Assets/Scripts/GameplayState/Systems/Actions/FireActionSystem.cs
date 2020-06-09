using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class FireActionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref ActingThisTurn _, ref FireAction fireAction, ref MapPositionComponent position, ref ActorComponent actor) => {
			// CREATE BULLET AND THAT'S REALLY IT
			var bullet = EntityManager.CreateEntity(typeof(Translation), typeof(Rotation), typeof(LocalToWorld), typeof(RenderMesh), typeof(RenderBounds), typeof(MapPositionComponent), typeof(BulletComponent), typeof(ActorComponent));
			var spawnPos = position.GetRelativePosition(0, -1);


			var bulletRenderMesh = EntityManager.GetSharedComponentData<RenderMesh>(entity);
			bulletRenderMesh.material = Resources.Load<Material>("Materials/BulletMaterial");

			var bulletRenderBounds = new AABB
			{
				Center = new float3(bulletRenderMesh.mesh.bounds.center),
				Extents = new float3(bulletRenderMesh.mesh.bounds.extents),
			};

			EntityManager.SetComponentData(bullet, new MapPositionComponent(position) { X = spawnPos.x, Y = spawnPos.y, DeltaX = 0, DeltaY = 0 });
			EntityManager.SetSharedComponentData(bullet, bulletRenderMesh);
			EntityManager.SetComponentData(bullet, new RenderBounds { Value = bulletRenderBounds });
			EntityManager.SetComponentData(bullet, new ActorComponent(1));

			actor = actor.ConsumeAction(1);
			EntityManager.RemoveComponent<FireAction>(entity);
		});
	}
}