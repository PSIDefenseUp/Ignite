using UnityEngine;

public class SceneMusic : MonoBehaviour
{
	public string Name;

	public void Start()
	{
		var musicPlayer = FindObjectOfType<MusicPlayer>();

		if (musicPlayer != null)
		{
			musicPlayer.PlayTrack(Name);
		}
	}
}