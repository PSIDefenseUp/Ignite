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

	public int2 GetRelativePosition(int2 relativeDelta)
	{
		return Value + GetAbsoluteOffset(relativeDelta);
	}

	public int2 GetAbsoluteOffset(int2 relativeDelta)
	{
		var relativeMoveAngle = Mathf.Atan2(relativeDelta.y, relativeDelta.x) * Mathf.Rad2Deg;
		var magnitude = Mathf.Sqrt(relativeDelta.y * relativeDelta.y + relativeDelta.x * relativeDelta.x);

		var worldAngle = (-Rotation + (360 + relativeMoveAngle)) % 360;

		var worldAngleXComponent = -Mathf.Cos(worldAngle * Mathf.Deg2Rad) * magnitude;
		var roundedXComponent = Mathf.RoundToInt(worldAngleXComponent);

		var worldAngleYComponent = Mathf.Sin(worldAngle * Mathf.Deg2Rad) * magnitude;
		var roundedYComponent = Mathf.RoundToInt(worldAngleYComponent);

		return new int2(roundedXComponent, roundedYComponent);
	}

	public float GetClosestAbsoluteDirection(int2 otherPosition, int granularity)
	{
		var diff = otherPosition - this.Value;
		var angle = (Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 270) % 360;
		var incrementsOf = 360 / granularity;

		var increments = (int)(angle / incrementsOf) + (angle % incrementsOf < (incrementsOf/2f) ? 0 : 1);
		var ret = increments * incrementsOf;

		return ret;
	}
}