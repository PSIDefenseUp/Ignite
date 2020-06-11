using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDirectionSystem 
{
    public void Tick()
    {
		var animators = Object.FindObjectsOfType<Animator>();
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        foreach(var animator in animators)
        {
            var position = animator.GetComponent<Position>();
            if(position != null)
            {
                animator.SetFloat("Dir", position.Rotation);
            }
        }
    }
}


