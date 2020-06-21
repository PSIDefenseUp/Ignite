using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Expressive : MonoBehaviour
{
    // Start is called before the first frame update
    public Dictionary<string,Sprite> Expressions;
    private Image myImage;
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeExpression(string Name)
    {
        myImage.sprite = Expressions[Name];
    }
}
