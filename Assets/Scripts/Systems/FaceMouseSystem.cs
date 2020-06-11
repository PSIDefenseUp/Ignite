using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMouseSystem 
{
    public void Tick()
    {
		var entities = Object.FindObjectsOfType<FaceMouse>();
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        foreach(var entity in entities)
        {
            var position = entity.GetComponent<Position>();
            // var state = entity.GetComponent<Animator>();
            if(position != null)
            {
                Vector3 curr = entity.transform.position;
                var angle = Mathf.Atan2(mousePosition.y - curr.y, mousePosition.x - curr.x) * Mathf.Rad2Deg;
                if (angle >= -135 && angle <= -45)
                {
                    position.Rotation = 180f;

                }
                else if (angle >= -45 && angle <= 45)
                {
                    position.Rotation = 270f;
                }
                else if (angle >= 45 && angle <= 135)
                {
                    position.Rotation = 0f;
                }
                else
                {
                    position.Rotation = 90f;
                }
            }
        }
    }
}


