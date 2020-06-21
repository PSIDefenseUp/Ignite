using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Expressive Left;
    public Expressive Right;
    private Expressive active;
    // Start is called before the first frame update
    void Start()
    {
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
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
        Set(expression, active);
        return this;
    }
    void ActivateLeft()
    {
        Right.gameObject.SetActive(false);
        Left.gameObject.SetActive(true);
        active = Left;
    }
    void ActivateRight()
    {
        Left.gameObject.SetActive(false);
        Left.gameObject.SetActive(true);
        active = Right;
    }
    void Set(string expression, Expressive e)
    {
        if(!e.ChangeExpression(expression))
        {
            Debug.Log("Not Found, Right Not Set to: " + expression);
        }
    }
}
