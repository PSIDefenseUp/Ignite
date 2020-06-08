using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class CameraFollowSystem : ComponentSystem
{
	private Camera camera;

	protected override void OnCreate()
	{
		camera = Object.FindObjectOfType<Camera>();
	}

	protected override void OnUpdate()
	{
		var target = GetSingletonEntity<MainCameraTargetComponent>();
		var targetWorldPosition = EntityManager.GetComponentData<Translation>(target);

		camera.transform.position = new Vector3(targetWorldPosition.Value.x, targetWorldPosition.Value.y, camera.transform.position.z);
	}
}