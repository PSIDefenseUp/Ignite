using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Command
{
    public string CommandName;
    public string[] Args;
}

public class ScriptParser
{
    private Scene Scene;
    private string script;
    private int index;
    private string commandRegex = "\\[(?<command>[A-Za-z]+)\\]\\((?<parameters>[\\w | .!?…~\"',-n’ ]*)\\)"; //"[COMMANDNAME](ARGSLIST)"
    private Regex regex;
    public ScriptParser(string filepath, Scene s)
    {
        //read contents of file and store it to the script
        
        regex = new Regex (commandRegex);
        Scene = s;
        var stream = new StreamReader(filepath);
        script = stream.ReadToEnd();
    }
    public Command? ReadNextCommand()
    {
        var remainingScript = script.Substring(index);
        var match = regex.Match(remainingScript);
        if(!match.Success)
        {
            return null;
        }
        index += match.Groups[0].Length;
        return new Command
        {
            CommandName = match.Groups[1].Value,
            Args = match.Groups[2].Value.Split('|').Select(arg => arg.Trim()).ToArray()
        };

    }
    public void Next()
    {
        var nextCommand = ReadNextCommand();
        if(nextCommand.HasValue)
        {
            ExecuteCommand(nextCommand.Value);
        }
        else
        {
            Debug.Log("There was a problem getting next command");
        }
    }
    public void ExecuteCommand(Command c)
    {
        switch(c.CommandName)
        {
            case "display":
                Scene.Display(c.Args[0], c.Args[1], c.Args[2]);
                Debug.Log("Display Invoked on: "+ c.Args[2]);
                break;
            case "say":
                Scene.Say(c.Args[0], c.Args[1]);
                Debug.Log("Say Invoked");
                Debug.Log("speaker: " + c.Args[1]);
                Debug.Log("Speech: "+ c.Args[0]);
                break;
            case "expression":
                Scene.Expression(c.Args[0], c.Args[1]);
                break;
            case "delete":

                Scene.Delete(c.Args[0]);
                break;
            case "load":
                Scene.Say("", "");
                SceneManager.LoadScene(c.Args[0]);
                break;
        }

    }
    
}