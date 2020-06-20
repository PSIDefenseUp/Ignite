using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{

	[HideInInspector]
	public Sprite[] Frames;
	[HideInInspector]
	public int CurrentFrame = 0;

	public float Framerate = 16;
	public bool Loop = true;

	private float FrameTime => 1 / Framerate;
	private float FrameProgress = 0;
	private new SpriteRenderer renderer;

	public void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	public void Update()
	{
		FrameProgress += Time.deltaTime;

		if (FrameProgress > FrameTime)
		{
			CurrentFrame = (CurrentFrame + (int)(FrameProgress / FrameTime)) % Frames.Length;
			FrameProgress %= FrameTime;
		}

		renderer.sprite = Frames[CurrentFrame];
	}
}