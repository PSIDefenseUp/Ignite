using Unity.Mathematics;
using UnityEngine;

public class SlipNSlide : MonoBehaviour
{
	public int2? start, end;

	public void Init(int2 start, int2 end)
	{
		this.start = start;
		this.end = end;
	}
}