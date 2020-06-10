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
		var absoluteDelta = position.GetAbsoluteOffset(delta);
		var moveAction = new MoveAction(absoluteDelta.x, absoluteDelta.y);
		moveAction.Perform(actor);
	}
}