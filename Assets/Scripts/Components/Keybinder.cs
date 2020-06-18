using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Keybinder : MonoBehaviour
{
	public InputCommand BindingCommand;

	private Button button;
	private Text buttonText;
	private bool isListeningForInput = false;

	// Start is called before the first frame update
	public void Start()
	{
		this.button = GetComponentInChildren<Button>();
		this.buttonText = button.GetComponentInChildren<Text>();

		button.onClick.AddListener(BeginListeningForInput);
	}

	// Update is called once per frame
	public void Update()
	{
		if (isListeningForInput)
		{
			if (Input.anyKeyDown)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					PlayerPrefs.GetString("Keybinds");

					var currentBind = InputSystem.Instance.GetKey(BindingCommand);
					buttonText.text = currentBind.ToString();

					isListeningForInput = false;
					EventSystem.current.SetSelectedGameObject(null);
					return;
				}
				else
				{
					foreach(KeyCode key in Enum.GetValues(typeof(KeyCode)))
					{
						if (Input.GetKeyDown(key))
						{
							InputSystem.Instance.Unbind(BindingCommand);
							InputSystem.Instance.Bind(key, BindingCommand);
							buttonText.text = key.ToString();
							isListeningForInput = false;
							EventSystem.current.SetSelectedGameObject(null);
							return;
						}
					}
				}
			}

			button.Select();
		}
		else
		{
			buttonText.text = InputSystem.Instance.GetKey(BindingCommand).ToString();
		}
	}

	private void BeginListeningForInput()
	{
		isListeningForInput = true;
		buttonText.text = "...";
	}
}
