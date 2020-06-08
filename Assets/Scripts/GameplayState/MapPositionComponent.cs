using System;
using Unity.Entities;
using Unity.Mathematics;

public struct MapPositionComponent : IComponentData
{
	public int X, Y, DeltaX, DeltaY;
	public double Rotation; // rotation in radians

	public MapPositionComponent(int x, int y)
	{
		this.X = x;
		this.Y = y;
		this.DeltaX = 0;
		this.DeltaY = 0;
		this.Rotation = 0;
	}

	public MapPositionComponent(MapPositionComponent other)
	{
		this.X = other.X;
		this.Y = other.Y;
		this.DeltaX = other.DeltaX;
		this.DeltaY = other.DeltaY;
		this.Rotation = other.Rotation;
	}

	public MapPositionComponent(MapPositionComponent baseComponent, int dx, int dy) : this(baseComponent)
	{
		this.DeltaX = dx;
		this.DeltaY = dy;
	}

	public MapPositionComponent SetRotation(double rotation)
	{
		return new MapPositionComponent(this) { Rotation = rotation };
	}

	public MapPositionComponent RotateBy(double rotation)
	{
		return new MapPositionComponent(this) { Rotation = (this.Rotation + rotation) % (2 * Math.PI) };
	}

	public float2 GetRelativePosition(int dx, int dy)
	{
		var relativeMoveAngle = Math.Atan2(dy, dx);
		var magnitude = Math.Sqrt(dy * dy + dx * dx);

		var worldAngle = this.Rotation + ((2 * Math.PI) + relativeMoveAngle) % (2 * Math.PI);
		var worldAngleXComponent = (int)(Math.Cos(worldAngle) * magnitude);
		var worldAngleYComponent = (int)(Math.Sin(worldAngle) * magnitude);

		return new float2(this.X + worldAngleXComponent, this.Y + worldAngleYComponent);
	}
}