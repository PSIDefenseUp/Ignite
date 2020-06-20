using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
public struct Command
{
    public string CommandName;
    public string[] Args;
}

public class ScriptParser
{
    public DialogueSystem DialogueSystem;
    public SpriteSelect Scene;
    private string script;
    private int index;
    private string commandRegex = "\\[(?<command>[A-Za-z]+)\\]\\((?<parameters>[A-Za-z ,]*)\\)"; //"[COMMANDNAME](ARGSLIST)"
    private Regex regex;
    public ScriptParser(string filepath)
    {
        //read contents of file and store it to the script
        script = @"[DISPLAY](Marisa, xcoord, ycoord)
[SPEAK](Marisa, SOME TEXT)
[Display](SomeoneElse, xcoord, ycoord)
[SPEAK](Marisa, SOME MORE TEXT)
[SPEAK](SomeoneElse, Hi im TEXT)
[ClearActors]()";    //delete this
        regex = new Regex (commandRegex);
    }
    public Command? ReadNextCommand()
    {
        var remainingScript = script.Substring(index);
        var match = regex.Match(remainingScript);
        if(!match.Success)
        {
            return null;
        }
        return new Command
        {
            CommandName = match.Groups[1].Value,
            Args = match.Groups[2].Value.Split(',').Select(arg => arg.Trim()).ToArray()
        };

    }
    
    public void ExecuteCommand(Command c)
    {
        switch(c.CommandName)
        {
            case "DISPLAY":
                Scene.DisplayActor(c.Args[0], float.Parse(c.Args[1]), float.Parse(c.Args[2]));
                break;
            case "SPEAK":
                DialogueSystem.Say(c.Args[1], c.Args[0]);
                break;
            case "CLEARACTORS":
                Scene.ClearScene();
                break;
        }

    }
    
}