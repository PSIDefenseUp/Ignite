using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDirectionSystem
{
	public void Tick()
	{
		var animators = Object.FindObjectsOfType<Animator>();

		foreach(var animator in animators)
		{
			var position = animator.GetComponent<Position>();
			if(position != null)
			{
				switch ((int)position.Rotation)
				{
					case 0:
					animator.SetTrigger("Up");
					break;
					case 90:
					animator.SetTrigger("Left");
					break;
					case 180:
					animator.SetTrigger("Down");
					break;
					case 270:
					animator.SetTrigger("Right");
					break;
				}
			}
		}
	}
}
