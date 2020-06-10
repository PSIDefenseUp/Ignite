using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapToWorldRotationSystem : MonoBehaviour
{
    public void Tick()
	{
		var rotatoes = Object.FindObjectsOfType<RotateWithPosition>();

		foreach (var rotato in rotatoes)
		{
            var position = rotato.GetComponent<Position>();
			position.transform.rotation = Quaternion.Euler(0, 0, position.Rotation);
		}
	}
}
