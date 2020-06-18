using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputSystem
{
	private const string KEYBINDS_PREF_KEY = "KEYBINDS";

	private static InputSystem instance;
	private static readonly object instanceLock = new object();
	public static InputSystem Instance
	{
		get
		{
			if (instance == null)
			{
				lock(instanceLock)
				{
					if (instance == null)
					{
						instance = new InputSystem();
					}
				}
			}

			return instance;
		}
	}

	private Dictionary<KeyCode, InputCommand> keybinds;

	private InputSystem()
	{
		LoadBinds();
	}

	~InputSystem()
	{
		SaveBinds();
	}

	private void LoadBinds()
	{
		var serializedKeybinds = PlayerPrefs.GetString(KEYBINDS_PREF_KEY);

		if (serializedKeybinds == null || serializedKeybinds.Length == 0)
		{
			ResetBinds();
		}
		else
		{
			// TODO: read keybinds from playerprefs and deserialize
		}
	}

	private void SaveBinds()
	{
		// TODO: save off serialized keybinds to playerprefs

		//string serializedKeybinds = "";
		//PlayerPrefs.SetString(KEYBINDS_PREF_KEY, serializedKeybinds);
	}

	public void Tick()
	{
		GameState.Instance.InputCommand = InputCommand.NONE;

		foreach(var bind in keybinds)
		{
			if (Input.GetKeyDown(bind.Key))
			{
				GameState.Instance.InputCommand = bind.Value;
			}
		}
	}

	public KeyCode GetKey(InputCommand command)
	{
		// get just the first one we come across I guess
		// we're only realistically supporting exclusive keybinds anyway
		return keybinds.Where(bind => bind.Value == command).Select(bind => bind.Key).FirstOrDefault();
	}

	public void Bind(KeyCode key, InputCommand command)
	{
		keybinds[key] = command;
	}

	public void Unbind(KeyCode key)
	{
		keybinds.Remove(key);
	}

	public void Unbind(InputCommand command)
	{
		var keysToRemove = keybinds.Where(bind => bind.Value == command).Select(bind => bind.Key).ToArray();

		foreach (var key in keysToRemove)
		{
			keybinds.Remove(key);
		}
	}

	public void ResetBinds()
	{
		keybinds = new Dictionary<KeyCode, InputCommand>()
		{
			[KeyCode.W] = InputCommand.MOVEUP,
			[KeyCode.A] = InputCommand.MOVELEFT,
			[KeyCode.S] = InputCommand.MOVEDOWN,
			[KeyCode.D] = InputCommand.MOVERIGHT,
			[KeyCode.Q] = InputCommand.STRAFELEFT,
			[KeyCode.E] = InputCommand.STRAFERIGHT,
			[KeyCode.Space] = InputCommand.PASSTURN,
			[KeyCode.Mouse0] = InputCommand.FIRE,
			[KeyCode.Mouse1] = InputCommand.ALTFIRE,
			[KeyCode.I] = InputCommand.MOVEFORWARD,
			[KeyCode.J] = InputCommand.TURNLEFT,
			[KeyCode.K] = InputCommand.MOVEBACK,
			[KeyCode.L] = InputCommand.TURNRIGHT,
		};
	}
}