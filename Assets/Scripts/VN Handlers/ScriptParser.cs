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
    private string commandRegex = "\\[(?<command>[A-Za-z]+)\\]\\((?<parameters>[\\w | .!?…~\"',-’* ]*)\\)"; //"[COMMANDNAME](ARGSLIST)"
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
        // Debug.Log("matches groups value: " + match.Groups[0].Value);
        index += match.Groups[0].Value.Length;
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
            Debug.Log(nextCommand.Value.CommandName + "Invoked");
            var args = nextCommand.Value.Args;
            foreach (string s in args)
            {
                Debug.Log(s);
            }
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
            case "Display":
                Scene.Display(c.Args[0], c.Args[1], c.Args[2]);
                Next();
                break;
            case "say":
            case "Say":
                Scene.Say(c.Args[0], c.Args[1]);
                break;
            case "expression":
            case "Expression":
                Scene.Expression(c.Args[0], c.Args[1]);
                Next();
                break;
            case "delete":
            case "Delete":
                Scene.Delete(c.Args[0]);
                Next();
                break;
            case "load":
            case "Load":
                Scene.Say("", "");
                SceneManager.LoadScene(c.Args[0]);
                break;
        }

    }
    
}