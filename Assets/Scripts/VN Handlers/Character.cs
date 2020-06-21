using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Expressive Left;
    private Expressive Right;
    private Expressive active;
    // Start is called before the first frame update
    void Awake()
    {
        Expressive[] children = this.GetComponentsInChildren<Expressive>();
        foreach(Expressive e in children)
        {
            if (e.gameObject.name == "Right")
            {
                Right = e;
                Debug.Log("Right Set");
            }
            else
            {
                Left = e;
                Debug.Log("Left Set");
            }
        }
        Left.Deactivate();
        Right.Deactivate();
    }
    public Character Instantiate(string expression, string side)
    {
        if(side == "Right")
        {
            ActivateRight();
        }
        else
        {
            ActivateLeft();
        }
        Set(expression);
        return this;
    }
    void ActivateLeft()
    {
        Right.Deactivate();
        Left.Activate();;
        active = Left;
    }
    void ActivateRight()
    {
        Left.Deactivate();
        Left.Activate();
        active = Right;
    }
    public void Set(string expression)
    {
        if(!active.ChangeExpression(expression))
        {
            Debug.Log("Not Found, Right Not Set to: " + expression);
        }
    }
}
