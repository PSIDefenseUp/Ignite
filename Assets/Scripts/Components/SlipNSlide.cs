using Unity.Mathematics;
using UnityEngine;

public class SlipNSlide : MonoBehaviour
{
	public int2? startPosition;
	public bool HasNotFinished;

	public void Init(int2 start)
	{
		this.startPosition = start;
		this.HasNotFinished = true;
	}

	public void Finish()
	{
		this.HasNotFinished = false;
	}
}