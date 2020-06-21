using UnityEngine;

public class ActionSprite : MonoBehaviour
{
	[HideInInspector]
	public bool Finished = true;
	public Sprite[] Up;
	public Sprite[] Down;
	public Sprite[] Left;
	public Sprite[] Right;

	public void Init(Sprite[] up, Sprite[] down, Sprite[] left, Sprite[] right)
	{
		this.Up = up;
		this.Down = down;
		this.Left = left;
		this.Right = right;
		this.Finished = false;
	}
}