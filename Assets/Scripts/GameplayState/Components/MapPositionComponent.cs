using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

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

	public MapPositionComponent SetMove(int dx, int dy)
	{
		return new MapPositionComponent(this) { DeltaX = dx, DeltaY = dy };
	}

	public MapPositionComponent SetMoveRelative(int dx, int dy)
	{
		var relativeOffset = GetRelativeOffset(dx, dy);

		return new MapPositionComponent(this) { DeltaX = relativeOffset.x, DeltaY = relativeOffset.y };
	}

	public MapPositionComponent Move()
	{
		return new MapPositionComponent(this) { X = X + DeltaX, Y = Y + DeltaY, DeltaX = 0, DeltaY = 0 };
	}

	public MapPositionComponent RotateBy(double rotation)
	{
		return new MapPositionComponent(this) { Rotation = (this.Rotation + rotation) % (2 * Math.PI) };
	}

	public int2 GetRelativeOffset(int dx, int dy)
	{
		var relativeMoveAngle = Math.Atan2(dy, dx);
		var magnitude = Math.Sqrt(dy * dy + dx * dx);

		var worldAngle = (-this.Rotation + ((2 * Math.PI) + relativeMoveAngle)) % (2 * Math.PI);
		var worldAngleXComponent = (int)(Math.Cos(worldAngle) * magnitude);
		var worldAngleYComponent = (int)(Math.Sin(worldAngle) * magnitude);

		return new int2(worldAngleXComponent, worldAngleYComponent);
	}

	public int2 GetRelativePosition(int dx, int dy)
	{
		return new int2(this.X, this.Y) + GetRelativeOffset(dx, dy);
	}
}