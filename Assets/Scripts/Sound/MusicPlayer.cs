using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
	private AudioSource audioSource;
	private float volume = 1;
	private string currentMusic;

	public void Awake()
	{
		if (FindObjectsOfType<MusicPlayer>().Length > 1 && currentMusic == null)
		{
			DestroyImmediate(this);
			return;
		}

		DontDestroyOnLoad(gameObject);

		if (PlayerPrefs.HasKey("MusicVolume"))
		{
			SetVolume(PlayerPrefs.GetFloat("MusicVolume"));
		}

		audioSource = GetComponent<AudioSource>();
	}

	public void SetVolume(float v)
	{
		volume = v;
		var sources = GetComponents<AudioSource>();
		foreach(AudioSource s in sources)
		{
			s.volume = volume;
		}
	}

	public void PlayTrack(string name)
	{
		if (currentMusic != null && currentMusic.Equals(name, StringComparison.InvariantCultureIgnoreCase))
		{
			return;
		}

		currentMusic = name;
		var music = Resources.Load<AudioClip>("Final/Music/" + name);

		audioSource.Stop();
		audioSource.loop = true;
		audioSource.clip = music;
		audioSource.volume = volume;
		audioSource.Play();
	}
}
