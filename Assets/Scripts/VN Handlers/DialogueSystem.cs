using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour 
{
	public static DialogueSystem instance;

	public ELEMENTS elements;

	void Awake()
	{
		instance = this;
	}

	void Start () 
	{
		
	}

	
	public void Say(string speech, string speaker = "")
	{
		StopSpeaking();

		speaking = StartCoroutine(Speaking(speech, false, speaker));
	}

	
	public void SayAdd(string speech, string speaker = "")
	{
		StopSpeaking();

		speechText.text = targetSpeech;

		speaking = StartCoroutine(Speaking(speech, true, speaker));
	}

	public void StopSpeaking()
	{
		if (isSpeaking)
		{
			StopCoroutine(speaking);
		}
		speaking = null;
	}
		
	public bool isSpeaking {get{return speaking != null;}}
	[HideInInspector] public bool isWaitingForUserInput = false;

	public string targetSpeech = "";
	Coroutine speaking = null;
	IEnumerator Speaking(string speech, bool additive, string speaker = "")
	{
		speechPanel.SetActive(true);
		targetSpeech = speech;

		if (!additive)
			speechText.text = "";
		else
			targetSpeech = speechText.text + targetSpeech;

		speakerNameText.text = DetermineSpeaker(speaker);//temporary

		isWaitingForUserInput = false;

		while(speechText.text != targetSpeech)
		{
			speechText.text += targetSpeech[speechText.text.Length];
			yield return new WaitForEndOfFrame();
		}

		//text finished
		isWaitingForUserInput = true;
		while(isWaitingForUserInput)
			yield return new WaitForEndOfFrame();

		StopSpeaking();
	}

	string DetermineSpeaker(string s)
	{
		string retVal = speakerNameText.text;//default return is the current name
		if (s != speakerNameText.text && s != "")
			retVal = (s.ToLower().Contains("narrator")) ? "" : s;

		return retVal;
	}

	[System.Serializable]
	public class ELEMENTS
	{
		public GameObject SpeechPanel;
		public TextMeshProUGUI SpeakerNameText;
		public TextMeshProUGUI SpeechText;
	}
	public GameObject speechPanel {get{return elements.SpeechPanel;}}
	public TextMeshProUGUI speakerNameText {get{return elements.SpeakerNameText;}}
	public TextMeshProUGUI speechText {get{return elements.SpeechText;}}
}