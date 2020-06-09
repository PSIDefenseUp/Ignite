using Unity.Entities;

public struct FireAction : IComponentData
{
	public double Direction;

	public FireAction(double direction)
	{
		this.Direction = direction;
	}
}