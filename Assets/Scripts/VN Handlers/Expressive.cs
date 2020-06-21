using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Expression
{
    public string Name;
    public Sprite Sprite;
}
public class Expressive : MonoBehaviour
{
    // Start is called before the first frame update
    // public Dictionary<string,Sprite> Expressions;
    public Expression[] Expressions;
    private Image myImage;
    void Start()
    {
    }
    public void Activate()
    {
    
        myImage = myImage ?? GetComponent<Image>();
        

    }
    public void Deactivate()
    {
        myImage = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool ChangeExpression(string Name)
    {
        // myImage.sprite = Expressions[Name];
        foreach (Expression e in Expressions)
        {
            if (e.Name == Name)
            {
                myImage.sprite = e.Sprite;
                return true;
            }
        }
        return false;

    }
}
