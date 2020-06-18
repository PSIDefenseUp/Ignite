using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	DialogueSystem dialogue;
	// Start is called before the first frame update
	void Start()
	{
		dialogue = DialogueSystem.instance;
	}


	// Update is called once per frame
	private string[] s = new string[]
	{
		"WAIII ACAB AKARI ACAB DAISUKE:Akari",
		"ACAB WAIIII:LAMO",
		"Google 40% Cops for a cool suprise",
	};

	int index = 0;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
			{
				Say(s[index]);
				index++;
			}
		}
	}

	void Say(string s)
	{
		string[] parts = s.Split(':');
		string speech = parts[0];
		string speaker = (parts.Length >= 2) ? parts[1] : "";
		dialogue.Say(speech, speaker);
	}
}
