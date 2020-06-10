using Unity.Mathematics;
using UnityEngine;

public class MoveRelativeAction : IAction
{
	private readonly int2 delta;

	public MoveRelativeAction(int dx, int dy)
	{
		this.delta = new int2(dx, dy);
	}

	public int GetCost()
	{
		return 1;
	}

	public void Perform(GameObject actor)
	{
		var position = actor.GetComponent<Position>();
		position.Value += position.GetAbsoluteOffset(delta);
	}
}