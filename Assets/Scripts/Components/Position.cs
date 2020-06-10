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
		var relativeMoveAngle = Mathf.Atan2(relativeDelta.y, relativeDelta.x) * Mathf.Rad2Deg;
		var magnitude = Mathf.Sqrt(relativeDelta.y * relativeDelta.y + relativeDelta.x * relativeDelta.x);
		var worldAngle = (-Rotation + (360 + relativeMoveAngle)) % 360;

		// holy shit this became so bad what a disaster
		// but hey eight directional movement!
		var worldAngleXComponent = -Mathf.Cos(worldAngle * Mathf.Deg2Rad) * magnitude;
		var roundedXComponent = Mathf.Abs(worldAngleXComponent) < .7 ? 0 : worldAngleXComponent > 0 ? Mathf.CeilToInt(worldAngleXComponent) : Mathf.FloorToInt(worldAngleXComponent);

		var worldAngleYComponent = Mathf.Sin(worldAngle * Mathf.Deg2Rad) * magnitude;
		var roundedYComponent = Mathf.Abs(worldAngleYComponent) < .7 ? 0 : worldAngleYComponent > 0 ? Mathf.CeilToInt(worldAngleYComponent) : Mathf.FloorToInt(worldAngleYComponent);

		return new int2(roundedXComponent, roundedYComponent);
	}
}