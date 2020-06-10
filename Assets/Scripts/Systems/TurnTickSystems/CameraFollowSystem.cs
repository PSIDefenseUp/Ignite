using UnityEngine;

public class CameraFollowSystem
{
	public void Tick()
	{
		var camera = Object.FindObjectOfType<Camera>();
		var cameraTarget = Object.FindObjectOfType<CameraTarget>();

		if (cameraTarget != null)
		{
			camera.transform.position = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y, camera.transform.position.z);
		}
	}
}