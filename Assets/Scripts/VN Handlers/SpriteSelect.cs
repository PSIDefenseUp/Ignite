using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSelect : MonoBehaviour
{    
    private GameObject[] actors;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayActor(string name, float x, float y)
    {
        
    }
    public void ClearScene()
    {
        foreach(GameObject actor in actors)
        {
            Destroy(actor);
        }
    }
    
}
