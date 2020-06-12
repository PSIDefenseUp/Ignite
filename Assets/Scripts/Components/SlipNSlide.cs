using Unity.Mathematics;
using UnityEngine;

public class SlipNSlide : MonoBehaviour
{
	public int2? startPosition;

	public void Init(int2 start)
	{
		this.startPosition = start;
	}
}