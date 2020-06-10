using Unity.Mathematics;
using UnityEngine;

public class Position : MonoBehaviour
{
	public int2 Value;
	public float Rotation;

	public void Init(Position other)
	{
		this.Value = other.Value;
		this.Rotation = other.Rotation;
	}

	public void Init(int2 value, float rotation)
	{
		this.Value = value;
		this.Rotation = rotation;
	}

	public int2 GetAbsoluteOffset(int2 relativeDelta)
	{
		var relativeMoveAngle = Mathf.Atan2(relativeDelta.y, relativeDelta.x);
		var magnitude = Mathf.Sqrt(relativeDelta.y * relativeDelta.y + relativeDelta.x * relativeDelta.x);

		var worldAngle = (-Rotation + ((2 * Mathf.PI) + relativeMoveAngle)) % (2 * Mathf.PI);
		var worldAngleXComponent = (int)(-Mathf.Cos(worldAngle) * magnitude);
		var worldAngleYComponent = (int)(Mathf.Sin(worldAngle) * magnitude);

		return new int2(worldAngleXComponent, worldAngleYComponent);
	}
}