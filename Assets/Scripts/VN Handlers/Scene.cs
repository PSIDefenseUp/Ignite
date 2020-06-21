using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{    
    private List<GameObject> actors;
    private GameObject newActor;
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
        newActor = Instantiate(Resources.Load<GameObject>("Prefabs/VN/"+name));
        newActor.GetComponent<Transform>().position.Set(x, y, 0);
        actors.Add(newActor);
    }
    public void ClearScene()
    {
        foreach(GameObject actor in actors)
        {
            Destroy(actor);
        }
    }
    
}
