using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Scene : MonoBehaviour
{    
    private GameObject newActor;
    private Dictionary<string, Character> actors;
    public DialogueSystem Dialogue;
    private ScriptParser script;
    public string ScriptPath;
    public GameObject CharacterLayer;
    void Start()
    {
        actors = new Dictionary<string, Character>();
        script = new ScriptParser(ScriptPath , this);
        script.Next();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
		{
			if(!Dialogue.isSpeaking || Dialogue.isWaitingForUserInput)
			{
				script.Next();
			}
		}

    }
    public void Display(string expression, string name, string side)
    {

        newActor = Instantiate(Resources.Load<GameObject>("Prefabs/VN/"+name),CharacterLayer.transform);
        Character newCharacter = newActor.GetComponent<Character>().Instantiate(expression, side);
        actors.Add(name, newCharacter);
    }    
    public void Say(string name, string content)
    {
        Debug.Log(content);
        Dialogue.Say(content, name);
    }
    public void Expression(string expression, string name)
    {
        actors[name].Set(expression);
    }
    public void Delete(string name)
    {
        if(actors.ContainsKey(name))
        {
            GameObject.Destroy(actors[name]);  
        }
        else
        {
            Debug.Log("Tried and failed to delete " + name);
        }
    }
}
