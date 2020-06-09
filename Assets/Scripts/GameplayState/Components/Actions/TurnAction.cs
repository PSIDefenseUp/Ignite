using Unity.Entities;

public struct TurnAction : IComponentData
{
	public double DesiredRotation;

	public TurnAction(double rotateBy)
	{
		this.DesiredRotation = rotateBy;
	}
}