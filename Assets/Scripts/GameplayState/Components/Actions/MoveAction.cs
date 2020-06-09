using Unity.Entities;

public struct MoveAction : IComponentData
{
	public int DeltaX, DeltaY;

	public MoveAction(int dx, int dy)
	{
		this.DeltaX = dx;
		this.DeltaY = dy;
	}
}